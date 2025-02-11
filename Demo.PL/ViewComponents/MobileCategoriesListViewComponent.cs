using Demo.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.ViewComponents
{
    public class MobileCategoriesListViewComponent:ViewComponent
    {
        private readonly ICategoryRepository categoryRepository;

        public MobileCategoriesListViewComponent(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await categoryRepository.GetAll();
            return View(categories);
        }
    }
}
