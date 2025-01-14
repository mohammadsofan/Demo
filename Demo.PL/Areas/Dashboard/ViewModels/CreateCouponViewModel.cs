using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Areas.Dashboard.ViewModels
{
    public class CreateCouponViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Code { get; set; } = null!;
        [Required]
        [Display(Name = "Discount type")]
        public string SelectedType { get; set; } = null!;
        public IEnumerable<SelectListItem>? Types { get; set; } = null!;
        [Required]
        public double Discount { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
