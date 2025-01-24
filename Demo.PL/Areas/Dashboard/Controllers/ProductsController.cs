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
        private readonly IProductColorRepository productColorRepository;
        private readonly ProductService productService;
        private readonly IImageRepository imageRepository;

        public ProductsController(IProductRepository productRepository, ISubCategoryRepository subCategoryRepository,ICategoryRepository categoryRepository,IProductColorRepository productColorRepository,IImageRepository imageRepository, ProductService productService)
        {
            this.productRepository = productRepository;
            this.subCategoryRepository = subCategoryRepository;
            this.categoryRepository = categoryRepository;
            this.productColorRepository = productColorRepository;
            this.productService = productService;
            this.imageRepository = imageRepository;
        }
        public async Task<IActionResult> Index(Guid? id)
        {
            try
            {
                var categories = await categoryRepository.GetAll();
                if(!categories.Any())
                {
                    ViewBag.Categories = null;
                    return View();
                }
                ViewBag.Categories = productService.GetCategories(categories);

                var products = await productRepository.GetByCategory(id ?? categories.First().Id);
                ViewBag.CategoryName = categories.FirstOrDefault(c => c.Id == (id ?? categories.First().Id))?.Name ?? "Unknown Category";

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
                if (ModelState.IsValid)
                {
                    Product product = new Product()
                    {
                        Id = Guid.NewGuid(),
                        CreatedAt = DateTime.UtcNow,
                        Name = model.Name,
                        Description = model.Description,
                        Quantity = model.Quantity,
                        InPublish = model.InPublish,
                        Discount = model.Discount,
                        Price = model.Price,
                        SubCategoryId = Guid.Parse(model.SelectedSubCategory),

                    };
                    await productRepository.Create(product);
                    foreach(var pc in model.ProductColors)
                    {
                        pc.ProductId = product.Id;
                        var productColor = new ProductColor()
                        {
                            Id = Guid.NewGuid(),
                            HexCode = pc.HexCode,
                            ProductId = product.Id,

                        };
                        await productColorRepository.Create(productColor);
                        
                        foreach(var img in pc.Images)
                        {
                           
                            var imageName = await FileHelper.UploadImage(img, "images");
                            Image image = new Image()
                            {
                                 Id= Guid.NewGuid(),
                                 Name = imageName,
                                 ProductColorId=productColor.Id,
                            };
                            await imageRepository.Create(image);
                        }
                    }

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
            catch(InvalidOperationException ex)
            {
                return BadRequest();
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
                foreach(var pc in product.ProductColors)
                {
                    foreach (var image in pc.Images)
                    {
                        FileHelper.DeleteImage("images", image.Name);
                    }
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
            var ProductVM = new EditProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                Discount = product.Discount,
                InPublish = product.InPublish,
                CreatedAt = product.CreatedAt,
                SelectedSubCategory = product.SubCategoryId.ToString(),
                SubCategories = productService.GetSubCategories(subCategories),
                ProductColors = product.ProductColors.Select(pc => new EditProductColorViewModel()
                {
                    Id=pc.Id,
                    HexCode = pc.HexCode,
                    ProductId = pc.ProductId,
                    Images = pc.Images.Select(img => new ImageViewModel()
                    {
                        Id =img.Id,
                        ProductColorId = pc.Id,
                        Name=img.Name,
                    }).ToList(),
                }).ToList()
            
            };
            return View(ProductVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var product = new Product()
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Description = model.Description,
                        Price = model.Price,
                        Quantity = model.Quantity,
                        Discount = model.Discount,
                        InPublish = model.InPublish,
                        SubCategoryId = Guid.Parse(model.SelectedSubCategory),
                        CreatedAt = model.CreatedAt,
                    };
                    if (model.ImagesToDelete is not null)
                    {
                        var imagesToDelete = model.ImagesToDelete.Split(",");
                        if (imagesToDelete.Length > 0)
                        {
                            foreach (var image in imagesToDelete)
                            {
                                var result = Guid.TryParse(image, out var imageId);
                                if (result)
                                {
                                    var img = await imageRepository.Get(imageId);
                                    if (img is not null)
                                    {
                                        FileHelper.DeleteImage("images", img.Name);
                                        await imageRepository.Delete(img);

                                    }
                                }

                            }
                        }
                    }

                    foreach (var pc in model.ProductColors)
                    {
                        var productColor = new ProductColor()
                        {
                            Id =pc.Id,
                            HexCode = pc.HexCode,
                            ProductId = product.Id,

                        };
                        await productColorRepository.Update(productColor);
                        if (pc.NewImages is not null)
                        {
                            foreach (var img in pc.NewImages)
                            {

                                var imageName = await FileHelper.UploadImage(img, "images");
                                Image image = new Image()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = imageName,
                                    ProductColorId = productColor.Id,
                                };
                                await imageRepository.Create(image);
                            }
                        }
                    }
                    await productRepository.Update(product);


                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    var subCategories = await subCategoryRepository.GetAllWithCategory();
                    model.SubCategories = productService.GetSubCategories(subCategories);
                    var pc = await productColorRepository.GetByProductId(model.Id);
                    model.ProductColors = pc.Select(pc => new EditProductColorViewModel()
                    {
                        Id = pc.Id,
                        HexCode = pc.HexCode,
                        ProductId = pc.ProductId,
                        Images = pc.Images.Select(img => new ImageViewModel()
                        {
                            Id = img.Id,
                            ProductColorId = pc.Id,
                            Name = img.Name,
                        }).ToList(),
                    }).ToList();

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
