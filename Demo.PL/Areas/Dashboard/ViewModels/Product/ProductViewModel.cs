using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Areas.Dashboard.ViewModels.Product
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public double Discount { get; set; }
        public int Quantity { get; set; }
        public bool InPublish { get; set; }
        [Display(Name = "Image")]
        public string MainImage { get; set; } = null!;
        [Display(Name = "Sub Category")]
        public string SubCategoryName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
