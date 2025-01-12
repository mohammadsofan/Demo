namespace Demo.PL.Areas.Dashboard.ViewModels
{
    public class CategoryDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public IEnumerable<SubCategoryViewModel> SubCategories { get; set; } = null!;
    }
}
