using Demo.DAL.Enums;
using Demo.DAL.Models;
using Demo.PL.Areas.Dashboard.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demo.PL.Areas.Dashboard.Services
{
    public class ProductService
    {
        public IEnumerable<SelectListItem> GetSubCategories(IEnumerable<SubCategory?> subCategories)
        {
            var sc = subCategories.Select(sc => new SelectListItem()
            {
                Text = $"{sc.Category.Name} - {sc.Name}",
                Value = sc.Id.ToString()
            }).ToList();
            return sc;
        }

        public IEnumerable<SelectListItem> GetCategories(IEnumerable<Category?> categories)
        {
            var c = categories.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();
            return c;
        }
        public IEnumerable<ProductViewModel> MapToProductVM(IEnumerable<Product> products)
        {
            return products.Select(p => new ProductViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                InPublish = p.InPublish,
                Quantity = p.Quantity,
                Discount = p.Discount,
                SubCategoryName = p.SubCategory.Name,
                CreatedAt = p.CreatedAt,
                MainImage = p.Images.FirstOrDefault().Name,

            }).ToList();
        }
        public Product MapToProduct(CreateProductViewModel product,bool isNew)
        {
            return new Product()
            {
                Id = isNew?Guid.NewGuid():product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                InPublish = product.InPublish,
                Quantity = product.Quantity,
                Discount = product.Discount,
                SubCategoryId = Guid.Parse(product.SelectedSubCategory),
                CreatedAt = isNew ? DateTime.UtcNow:product.CreatedAt,
             };
        }
    }
}
