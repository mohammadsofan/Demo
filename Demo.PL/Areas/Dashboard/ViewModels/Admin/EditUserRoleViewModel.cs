using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Areas.Dashboard.ViewModels.Admin
{
    public class EditUserRoleViewModel
    {
        public string Id { get; set; } = null!;
        public string? UserName { get; set; } = null!;
        [Display(Name = "Select role")]
        public string SelectedRole { get; set; } = null!;
        public IEnumerable<SelectListItem>? Roles { get; set; } = null!;
    }
}
