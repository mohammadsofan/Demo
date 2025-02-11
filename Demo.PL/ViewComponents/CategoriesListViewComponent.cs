using Demo.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.ViewComponents
{
    public class CategoriesListViewComponent:ViewComponent
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesListViewComponent(ICategoryRepository categoryRepository)
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
