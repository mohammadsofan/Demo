using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Demo.PL.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly IMapper mapper;

        public AccountsController(IAccountRepository accountRepository,IMapper mapper) {
            this.accountRepository = accountRepository;
            this.mapper = mapper;
        }
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var user = mapper.Map<ApplicationUser>(model);
                    var result = await accountRepository.Register(user, model.Password);
                    if (result.Succeeded)
                    {
                        await accountRepository.AssignUserRole(user, "User");
                        return RedirectToAction(nameof(Login));

                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "An error occurred during registration. Please try again.");
                }
            }
            return View(model);
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await accountRepository.Login(model.UserName, model.Password, model.RememberMe);
                    if (result.Succeeded)
                    {
                        if (User.IsInRole("Admin"))
                        {
                           return RedirectToAction("CreateRole", "Admin", new { area="Dashboard"});
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {

                        ModelState.AddModelError(string.Empty, "Invalid username or password. Please try again.");

                    }
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "An error occurred during Login. Please try again.");

                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await accountRepository.Logout();
                return RedirectToAction(nameof(Login));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
