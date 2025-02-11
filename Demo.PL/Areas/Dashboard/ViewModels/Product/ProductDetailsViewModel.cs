using Demo.PL.Areas.Dashboard.ViewModels.ProductColor;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Areas.Dashboard.ViewModels.Product
{
    public class ProductDetailsViewModel
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public double Discount { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public List<ProductColorDetailsViewModel> ProductColors { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string SubCategory { get; set; } = null!;
        public string Category { get; set; } = null!;
    }
}
