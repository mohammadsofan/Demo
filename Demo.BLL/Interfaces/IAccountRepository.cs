using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IAccountRepository
    {
        Task<IdentityResult> Register(ApplicationUser user, string password);
        Task<SignInResult> Login(string userName, string password, bool rememberMe);
        Task Logout();
        Task<IdentityResult> CreateRole(string Name);
        Task<IdentityResult> AssignUserRole(ApplicationUser user,string roleName);
        IEnumerable<IdentityRole> GetRoles();
        Task<IdentityRole> GetRole(string id);
        Task<IEnumerable<string>> GetUserRoles(ApplicationUser user);
        Task<IdentityResult> DeleteRole(string id);
        IEnumerable<ApplicationUser> GetUsers();
        Task<ApplicationUser> GetUser(string id);


    }
}
