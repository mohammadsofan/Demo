namespace Demo.PL.Areas.Dashboard.ViewModels.SubCategory
{
    public class CreateSubCategoryViewModel
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IFormFile Image { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
