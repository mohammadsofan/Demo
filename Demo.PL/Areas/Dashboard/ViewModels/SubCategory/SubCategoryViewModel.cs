using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Areas.Dashboard.ViewModels.SubCategory
{
    public class SubCategoryViewModel
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Image {  get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
