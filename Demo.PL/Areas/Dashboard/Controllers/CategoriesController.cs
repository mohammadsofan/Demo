using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Areas.Dashboard.ViewModels.Category;
using Demo.PL.Areas.Dashboard.ViewModels.SubCategory;
using Demo.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demo.PL.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly ISubCategoryRepository subCategoryRepository;
        private readonly IMapper mapper;

        public CategoriesController(ICategoryRepository categoryRepository, ISubCategoryRepository subCategoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
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
                var categories = await categoryRepository.GetAll();
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
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var category = mapper.Map<Category>(model);
                    category.Id = Guid.NewGuid();
                    category.CreatedAt = DateTime.UtcNow;
                    category.Image = await FileHelper.UploadImage(model.Image, "images");
                    var result = await categoryRepository.Create(category);
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
        public async Task<IActionResult> Edit(Guid? id)
        {
            if(id is null)
            {
                return BadRequest();
            }
            var category = await categoryRepository.Get(id.Value);
            if(category is null)
            {
                return NotFound();
            }
            var categoryVM = mapper.Map<EditCategoryViewModel>(category);
            return View(categoryVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(model.NewImage is not null)
                    {
                        FileHelper.DeleteImage("images", model.Image ?? "");
                        model.Image = await FileHelper.UploadImage(model.NewImage, "images");
                    }
                    var Category = mapper.Map<Category>(model);
                    var result = await categoryRepository.Update(Category);
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

                var category = await categoryRepository.Get(id.Value);
                if (category is null)
                {
                    return NotFound();
                }
                ViewBag.CategoryName = category.Name;
                return View(id);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(Guid? id)
        {
            try
            {
                if (id is null)
                {
                    return BadRequest();
                }

                var category = await categoryRepository.Get(id.Value);
                if (category is null)
                {
                    return NotFound();
                }
                 FileHelper.DeleteImage("images", category.Image);
                await categoryRepository.Delete(category);
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
                var category = await categoryRepository.Details(id.Value);
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
                    Image=category.Image,
                    SubCategories = category.SubCategories.Select(sc => new SubCategoryViewModel()
                    {
                        CategoryId = id.Value,
                        Id = sc.Id,
                        Name = sc.Name,
                        Description = sc.Description,
                        Image= sc.Image,
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
            var category = await categoryRepository.Get(id.Value);
            if (category is null)
            {
                return NotFound();
            }
            var model = new CreateSubCategoryViewModel()
            {
                CategoryId = id.Value,
            };
            ViewBag.CategoryName = category.Name;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubCategory(CreateSubCategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var subCategory = mapper.Map<SubCategory>(model);
                    subCategory.CreatedAt = DateTime.UtcNow;
                    subCategory.Id = Guid.NewGuid();
                    subCategory.Image = await FileHelper.UploadImage(model.Image, "images");
                    var result = await subCategoryRepository.Create(subCategory);
                    if (result == 1)
                    {
                        return RedirectToAction(nameof(Details), new { id = model.CategoryId });
                    }
                    else
                    {
                        var category = await categoryRepository.Get(model.CategoryId);
                        ViewBag.CategoryName = category?.Name??"UnKnown Category";
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
                ViewBag.CategoryId = subCategory.CategoryId;
                ViewBag.SubCategoryName = subCategory.Name;
                return View(id);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteSubCategory(Guid? id)
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
                FileHelper.DeleteImage("images", subCategory.Image);
                await subCategoryRepository.Delete(subCategory);
                return RedirectToAction(nameof(Details), new { id = subCategory.CategoryId });
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
                var subCategoryVM = mapper.Map<EditSubCategoryViewModel>(subCategory);
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
        public async Task<IActionResult> EditSubCategory(EditSubCategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.NewImage is not null)
                    {
                        FileHelper.DeleteImage("images", model.Image ?? "");
                        var newImage = await FileHelper.UploadImage(model.NewImage, "images");
                        model.Image = newImage;
                    }
                    var subCategory = mapper.Map<SubCategory>(model);
                    var result = await subCategoryRepository.Update(subCategory);
                    if (result == 1)
                    {
                        return RedirectToAction(nameof(Details), new { id = model.CategoryId });
                    }
                    else
                    {
                        var category = await categoryRepository.Get(model.CategoryId);
                        ViewBag.categoryName = category?.Name;

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
