using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Areas.Dashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demo.PL.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRespository categoryRespository;
        private readonly ISubCategoryRepository subCategoryRepository;
        private readonly IMapper mapper;

        public CategoriesController(ICategoryRespository categoryRespository, ISubCategoryRepository subCategoryRepository, IMapper mapper)
        {
            this.categoryRespository = categoryRespository;
            this.subCategoryRepository = subCategoryRepository;
            this.mapper = mapper;
        }
        [HttpGet("/Dashboard/Categories")]
        public IActionResult RedirectToIndex()
        {
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Index()
        {

            try
            {
                var categories = await categoryRespository.GetAll();
                var categoriesVM = mapper.Map<IEnumerable<CategoryViewModel>>(categories);
                return View(categoriesVM);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var category = mapper.Map<Category>(model);
                    category.Id = Guid.NewGuid();
                    category.CreatedAt = DateTime.UtcNow;

                    var result = await categoryRespository.Create(category);
                    if (result == 1)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("Name", "this name already exists.");
                        return View(model);

                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

        }
        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await categoryRespository.Get(id);
            var categoryVM = mapper.Map<CategoryViewModel>(category);
            return View(categoryVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Category = mapper.Map<Category>(model);
                    var result = await categoryRespository.Update(Category);
                    if (result == 1)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("Name", "this name already exists.");
                        return View(model);

                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            try
            {
                if (id is null)
                {
                    return BadRequest();
                }

                var category = await categoryRespository.Get(id.Value);
                if (category is null)
                {
                    return NotFound();
                }
                var categoryVM = mapper.Map<CategoryViewModel>(category);
                return View(categoryVM);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(CategoryViewModel model)
        {
            try
            {
                var category = mapper.Map<Category>(model);
                await categoryRespository.Delete(category);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            try
            {
                if (id is null)
                {
                    return BadRequest();
                }
                var category = await categoryRespository.Details(id.Value);
                if (category is null)
                {
                    return NotFound();
                }

                var categoryDetailsVM = new CategoryDetailsViewModel()
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    CreatedAt = category.CreatedAt,
                    SubCategories = category.SubCategories.Select(sc => new SubCategoryViewModel()
                    {
                        CategoryId = id.Value,
                        Id = sc.Id,
                        Name = sc.Name,
                        Description = sc.Description,
                        CreatedAt = sc.CreatedAt
                    }).ToList(),
                };
                return View(categoryDetailsVM);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> CreateSubCategory(Guid? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var category = await categoryRespository.Get(id.Value);
            if (category is null)
            {
                return NotFound();
            }
            var model = new SubCategoryViewModel()
            {
                CategoryId = id.Value,
            };
            ViewBag.CategoryName = category.Name;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubCategory(SubCategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var subCategory = mapper.Map<SubCategory>(model);
                    subCategory.CreatedAt = DateTime.UtcNow;
                    subCategory.Id = Guid.NewGuid();
                    var result = await subCategoryRepository.Create(subCategory);
                    if (result == 1)
                    {
                        return RedirectToAction(nameof(Details), new { id = model.CategoryId });
                    }
                    else
                    {
                        var category = await categoryRespository.Get(model.CategoryId);
                        ViewBag.CategoryName = category.Name;
                        ModelState.AddModelError("Name", "this name already exists.");
                        return View(model);
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        public async Task<IActionResult> DeleteSubCategory(Guid? id)
        {
            try
            {
                if (id is null)
                {
                    return BadRequest();
                }
                var subCategory = await subCategoryRepository.Get(id.Value);
                if (subCategory is null)
                {
                    return NotFound();
                }
                var subCategoryVM = mapper.Map<SubCategoryViewModel>(subCategory);

                return View(subCategoryVM);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteSubCategory(SubCategoryViewModel model)
        {
            try
            {
                var subCategory = mapper.Map<SubCategory>(model);
                await subCategoryRepository.Delete(subCategory);
                return RedirectToAction(nameof(Details), new { id = model.CategoryId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> EditSubCategory(Guid? id,string categoryName)
        {
            try
            {
                if (id is null)
                {
                    return BadRequest();
                }
                var subCategory = await subCategoryRepository.Get(id.Value);
                if (subCategory is null)
                {
                    return NotFound();
                }
                var subCategoryVM = mapper.Map<SubCategoryViewModel>(subCategory);
                ViewBag.categoryName = categoryName;
                return View(subCategoryVM);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubCategory(SubCategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var subCategory = mapper.Map<SubCategory>(model);
                    var result = await subCategoryRepository.Update(subCategory);
                    if (result == 1)
                    {
                        return RedirectToAction(nameof(Details), new { id = model.CategoryId });
                    }
                    else
                    {
                        var category = await categoryRespository.Get(model.CategoryId);
                        ViewBag.categoryName = category.Name;

                        ModelState.AddModelError("Name", "this name already exists.");
                        return View(model);

                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
