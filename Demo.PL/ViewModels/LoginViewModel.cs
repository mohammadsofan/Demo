using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class LoginViewModel
    {
        [DataType("varchar(255)")]
        [MinLength(4,ErrorMessage ="Minimum Length is 4 Characters..!!")]
        [MaxLength(50,ErrorMessage = "Maximum Length is 50 Characters..!!")]
        [Required(ErrorMessage = "UserName Is Required..!!")]
        public string UserName { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password Is Required..!!")]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}
