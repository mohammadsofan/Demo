using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Areas.Dashboard.ViewModels
{
    public class RoleViewModel
    {
        public string? Id { get; set; } = null!;
        [Required(ErrorMessage = "Role name is required.")]
        public string Name { get; set; } = null!;
    }
}
