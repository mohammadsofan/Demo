using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class RegisterViewModel
    {
        [DataType("varchar(255)")]
        [MinLength(4, ErrorMessage = "Minimum Length is 4 Characters..!!")]
        [MaxLength(50, ErrorMessage = "Maximum Length is 50 Characters..!!")]
        [Required(ErrorMessage ="UserName Is Required..!!")]
        public string UserName { get; set; } = null!;
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email Is Required..!!")]
        public string Email { get; set; } = null!;
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password Is Required..!!")]
        [MinLength(8, ErrorMessage = "Minimum Length is 8 Characters..!!")]
        public string Password { get; set; } = null!;
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password Is Required..!!")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
        [DataType("varchar(15)")]
        [MaxLength(15,ErrorMessage = "Maximum Length is 15 Characters..!!")]
        [Required(ErrorMessage = "Phone  Is Required..!!")]
        public string PhoneNumber { get; set; } = null!;
        [DataType("varchar(255)")]
        [Required(ErrorMessage = "Address  Is Required..!!")]
        public string Address { get; set; } = null!;

    }
}
