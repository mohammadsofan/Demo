using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Areas.Dashboard.ViewModels.Category
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Image { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
