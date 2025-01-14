using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountRepository(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager,SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }
        public async Task<SignInResult> Login(string userName,string password,bool rememberMe)
        {
            try
            {
                var result = await signInManager.PasswordSignInAsync(userName, password, rememberMe, false);
                
                return result;
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("An error occurred while attempting to LogIn.", ex);
            }

        }

        public async Task Logout()
        {
            try
            {
                await signInManager.SignOutAsync();
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("An error occurred while attempting to Logout.", ex);

            }
        }

        public async Task<IdentityResult> Register(ApplicationUser user,string password)
        {
            try
            {
                user.CreatedAt= DateTime.UtcNow;
                var result = await userManager.CreateAsync(user, password);
                return result;
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("An error occurred while attempting to register the user.", ex);

            }


        }
        public async Task<IdentityResult> CreateRole(string roleName)
        {
            try
            {
                var result = await roleManager.CreateAsync(new IdentityRole() { Name = roleName });
                return result;
            }catch(Exception ex)
            {
                throw new InvalidOperationException("An error occurred while attempting to create the role.",ex);
            }
        }
        public async Task<IdentityResult> AssignUserRole(ApplicationUser user,string roleName)
        {
            try
            {
                var userRules= await GetUserRoles(user);
                await userManager.RemoveFromRolesAsync(user, userRules);
                var result = await userManager.AddToRoleAsync(user, roleName);
                
                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while attempting to assign the role to the user.", ex);
            }
        }
        public IEnumerable<IdentityRole> GetRoles()
        {
            try
            {
                return roleManager.Roles.ToList();
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("An error occurred while attempting to get the roles.", ex);
            }
        }
        public async Task<IdentityRole?> GetRole(string id)
        {
            try
            {
                var role= await roleManager.FindByIdAsync(id);
                
                return role;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while attempting to get the roles.", ex);
            }
        }
        public async Task<IEnumerable<string>> GetUserRoles(ApplicationUser user)
        {
            try
            {
                return await userManager.GetRolesAsync(user);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while attempting to get the roles.", ex);
            }
        }
        public async Task<IdentityResult> DeleteRole(string id)
        {
            try
            {
                var role= await roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    throw new InvalidOperationException("An error occurred while attempting to find the role.");
                }
                var result= await roleManager.DeleteAsync(role);
                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while attempting to delete the role.", ex);
            }
        }
        public IEnumerable<ApplicationUser> GetUsers()
        {
            try
            {
                return userManager.Users.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while attempting to get the users.", ex);
            }
        }
        public async Task<ApplicationUser?> GetUser(string id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);
               
                return user;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while attempting to get the users.", ex);
            }
        }

    }
}
