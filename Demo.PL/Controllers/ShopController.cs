using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.PL.Areas.Dashboard.ViewModels.Product;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Demo.PL.Controllers
{
    public class ShopController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly double productsPerPage = 8.0;

        public ShopController(ICategoryRepository categoryRepository,IProductRepository productRepository,IMapper mapper) {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await categoryRepository.GetAll();

                var shopVM = new ShopViewModel()
                {
                    Categories = categories!,
                };
                return View(shopVM);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public async Task<IActionResult> Product(Guid? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var product = await productRepository.Get(id.Value);
            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> GetPaginatedProducts(Guid? categoryId,Guid? subCategoryId,int page,int pageSize)
        {
            try
            {
                var products = await productRepository.GetPaginatedProducts(categoryId, subCategoryId, page, pageSize);
                var productsVm = products.Select(p => new ProductViewModel()
                {
                     Name=p.Name,
                     Description=p.Description,
                     Id=p.Id,
                     Price=p.Price,
                     Discount = p.Discount,
                     MainImage = p.ProductColors.First().Images.First().Name,
                     SubCategoryName=p.SubCategory.Name,
                     InPublish =p.InPublish,
                     Quantity = p.Quantity,
                     CreatedAt=p.CreatedAt.ToLocalTime(),
                }).ToList();
               
                var productsCount = await productRepository.GetPaginationProductsCount(categoryId, subCategoryId);
                return Json(new {success=true,products = productsVm, count = productsCount });
            }
            catch(Exception ex){
                await Console.Out.WriteLineAsync(ex.Message);
                return Json(new { success = false,message="failed to get products" });           
            }
        }
    }
}
