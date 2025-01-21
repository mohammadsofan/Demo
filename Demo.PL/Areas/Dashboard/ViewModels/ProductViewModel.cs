using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Areas.Dashboard.ViewModels
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
        public string MainImage { get; set; } = null!;
        public string SubCategoryName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
