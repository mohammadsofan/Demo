using Demo.DAL.Models;
using Demo.PL.Areas.Dashboard.ViewModels.ProductColor;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Areas.Dashboard.ViewModels.Product
{
    public class CreateProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        [Range(0.00, 1.00)]
        public double Discount { get; set; }
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        public bool InPublish { get; set; }
        public List<ProductColorViewModel> ProductColors { get; set; } = null!;
        public IEnumerable<SelectListItem>? SubCategories { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string SelectedSubCategory { get; set; } = null!;
    }
}
