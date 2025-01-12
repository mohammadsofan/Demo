using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.PL.Areas.Dashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Demo.PL.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly IMapper mapper;

        public AdminController(IAccountRepository accountRepository,IMapper mapper) {
            this.accountRepository = accountRepository;
            this.mapper = mapper;
        }

        public IActionResult CreateRole()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await accountRepository.CreateRole(model.Name);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Roles));
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (InvalidOperationException e)
                {
                    return BadRequest(e.Message);
                }
            }
            return View(model);
        }

        public IActionResult Roles()
        {
            try
            {
                var roles = accountRepository.GetRoles();
                var rolesVM = mapper.Map<IEnumerable<RoleViewModel>>(roles);
                return View(rolesVM);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }
        public async Task<IActionResult> DeleteRole(string? id)
        {
            try
            {
                if (id is null)
                {
                    return BadRequest();
                }

                var role = await accountRepository.GetRole(id);
                if (role is null)
                {
                    return NotFound();

                }
                var roleVM = mapper.Map<RoleViewModel>(role);
                return View("DeleteRole", roleVM);

            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoleConfirm(RoleViewModel model)
        {
            try
            {
                await accountRepository.DeleteRole(model.Id);
                return RedirectToAction(nameof(Roles));   
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }
        public async Task<IActionResult> Users()
        {
            try
            {
                var users = accountRepository.GetUsers();
                var usersVM = new List<UserViewModel>();
                foreach (var user in users)
                {
                    usersVM.Add(new UserViewModel()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address,
                        CreatedAt = user.CreatedAt,
                        RolesList = await accountRepository.GetUserRoles(user)

                    });
                }

                return View(usersVM);
            }
            catch(InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }
        public async Task<IActionResult> EditUserRole(string? id)
        {
            try
            {
                if (id is null)
                {
                    return BadRequest();
                }
                var user = await accountRepository.GetUser(id);
                if (user is null)
                {
                    return NotFound();
                }
                var editUserRoleVM = new EditUserRoleViewModel()
                {
                    Id = id,
                    UserName = user.UserName,
                    Roles = accountRepository.GetRoles().Select(r => new SelectListItem()
                    {
                        Text = r.Name,
                        Value = r.Id
                    }).ToList(),

                };
                return View(editUserRoleVM);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserRole(EditUserRoleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await accountRepository.GetUser(model.Id);
                    if (user is null)
                    {
                        ModelState.AddModelError(string.Empty, "user not found.");
                        return View(model);
                    }
                    var role = await accountRepository.GetRole(model.SelectedRole);
                    if (role is null)
                    {
                        ModelState.AddModelError(string.Empty, "role not found.");
                        return View(model);
                    }
                    var result = await accountRepository.AssignUserRole(user, role.Name);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "fail to edit user role .");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "fail to edit user role .");
                    return View(model);
                }

                return RedirectToAction(nameof(Users));
            }
            catch(InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
