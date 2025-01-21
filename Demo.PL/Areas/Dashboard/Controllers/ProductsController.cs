using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Areas.Dashboard.Services;
using Demo.PL.Areas.Dashboard.ViewModels;
using Demo.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demo.PL.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles ="Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ISubCategoryRepository subCategoryRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ProductService productService;
        private readonly IImageRepository imageRepository;

        public ProductsController(IProductRepository productRepository, ISubCategoryRepository subCategoryRepository,ICategoryRepository categoryRepository,ProductService productService,IImageRepository imageRepository)
        {
            this.productRepository = productRepository;
            this.subCategoryRepository = subCategoryRepository;
            this.categoryRepository = categoryRepository;
            this.productService = productService;
            this.imageRepository = imageRepository;
        }
        public async Task<IActionResult> Index(Guid? id)
        {
            try
            {
                 var categories = await categoryRepository.GetAll();
                if(categories == null || !categories.Any())
                {
                    ViewBag.Categories = null;
                    return View(null);
                }
                 ViewBag.Categories = productService.GetCategories(categories);
                 var products = await productRepository.GetByCategory(id);
                // in case the user selected a category that has no products - get the category name where category's id equals to the request id
                if (!products.Any() && id != null)
                    ViewBag.CategoryName = productService.GetCategories(categories).Where(item => Guid.Parse(item.Value) == id).Select(item => item.Text).First();
                else if(!products.Any() && id == null)
                {
                    ViewBag.CategoryName = productService.GetCategories(categories).Select(item => item.Text).First();
                }
                // in case the user enterd the index page without ant id- get the category name from the products
                else
                    ViewBag.CategoryName = products.First().SubCategory.Category.Name;

                var productsVm = productService.MapToProductVM(products);
               
                

                 return View(productsVm);
              
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Create()
        {
            var subCategories = await subCategoryRepository.GetAllWithCategory();
            var ProductVM = new CreateProductViewModel()
            {
                SubCategories = productService.GetSubCategories(subCategories)
            };
            return View(ProductVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            try
            {

                if (ModelState.IsValid) {
                    var product = productService.MapToProduct(model,true);
                    foreach (var item in model.Images)
                    {
                        var imageName = await FileHelper.UploadImage(item, "images");
                        var image=new Image()
                        {
                            Id = Guid.NewGuid(),
                            Name = imageName,
                            ProductId = product.Id
                        };
                        await imageRepository.Create(image);
                    }
                    await productRepository.Create(product);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var subCategories = await subCategoryRepository.GetAllWithCategory();
                    var ProductVM = new CreateProductViewModel()
                    {
                        SubCategories = productService.GetSubCategories(subCategories)
                    };
                    return View(ProductVM);
                }

            }
            catch (ArgumentException ex)
            {
                foreach (var image in model.Images)
                {
                    FileHelper.DeleteImage("images", image.Name);
                }
                var subCategories = await subCategoryRepository.GetAllWithCategory();

                model.SubCategories = productService.GetSubCategories(subCategories);
               
                ModelState.AddModelError("Images", ex.Message);
                return View(model);
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
                var product = await productRepository.Get(id.Value);
                if (product is null)
                {
                    return NotFound();
                }
                ViewBag.ProductName = product.Name;
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

                var product = await productRepository.Get(id.Value);
                if (product is null)
                {
                    return NotFound();
                }
                foreach(var image in product.Images)
                {
                    FileHelper.DeleteImage("images", image.Name);
                }
                await productRepository.Delete(product);
                return RedirectToAction(nameof(Index));
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
            var product = await productRepository.Get(id.Value);
            if(product is null)
            {
                return NotFound();
            }
            var subCategories = await subCategoryRepository.GetAllWithCategory();
            var ProductVM = new CreateProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price= product.Price,
                Quantity = product.Quantity,
                Discount = product.Discount,
                InPublish = product.InPublish,
                CreatedAt = product.CreatedAt,
                SelectedSubCategory=product.SubCategoryId.ToString(),
                SubCategories = productService.GetSubCategories(subCategories)
            
            };
            return View(ProductVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateProductViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var product = productService.MapToProduct(model,false);
                    await productRepository.Update(product);
                    return RedirectToAction(nameof(Index));
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
