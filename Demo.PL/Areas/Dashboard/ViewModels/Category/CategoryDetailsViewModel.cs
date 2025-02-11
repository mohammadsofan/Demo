using Demo.PL.Areas.Dashboard.ViewModels.SubCategory;

namespace Demo.PL.Areas.Dashboard.ViewModels.Category
{
    public class CategoryDetailsViewModel : CategoryViewModel
    {
        public IEnumerable<SubCategoryViewModel> SubCategories { get; set; } = null!;
    }
}
