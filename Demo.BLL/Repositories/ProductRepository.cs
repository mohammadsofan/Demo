using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> Create(Product product)
        {
            try
            {
                await dbContext.Products.AddAsync(product);
                await dbContext.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while createing the product. Please try again later.", ex);
            }
        }

        public async Task<Guid> Delete(Product product)
        {
            try
            {
                dbContext.Products.Remove(product);
                await dbContext.SaveChangesAsync();
                return product.Id;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while Deleting the product. Please try again later.", ex);
            }
        }

        public Task<Product> Details(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Product?> Get(Guid id)
        {
            try
            {
                var result = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the product. Please try again later.", ex);
            }
        }

        public async Task<IEnumerable<Product>> GetByCategory(Guid categoryId)
        {
            try
            {           
                var products = await dbContext.Products.Include(p => p.SubCategory).ThenInclude(sc => sc.Category).Where(s=>s.SubCategory.CategoryId == categoryId).AsNoTracking().ToListAsync();
                return products;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the products. Please try again later.", ex);
            }
        }
        public async Task<int> GetPaginationProductsCount(Guid? categoryId, Guid? subCategoryId)
        {
            try
            {
                if (subCategoryId.HasValue)
                {
                    return await dbContext.Products.Where(p=>p.SubCategoryId == subCategoryId).CountAsync();
                }
                else if (categoryId.HasValue)
                {
                    return await dbContext.Products.Where(p => p.SubCategory.CategoryId == categoryId).CountAsync();
                }
                else
                {
                    return await dbContext.Products.CountAsync();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the products count. Please try again later.", ex);
            }
        }

        public async Task<IEnumerable<Product>> GetPaginatedProducts(Guid? categoryId,Guid? subCategoryId,int page,int pageSize)
        {
            try
            {
                if(page < 1 || pageSize < 1)
                {
                    throw new ArgumentException("Page and pageSize must be greater than zero.");
                }
                int productsCount;
                IEnumerable<Product> products;
                IQueryable<Product> query = dbContext.Products.AsNoTracking().OrderByDescending(p=>p.CreatedAt);
                if (subCategoryId.HasValue)
                {
                    query = query.Where(p => p.SubCategory.Id == subCategoryId);
                }
                else if (categoryId.HasValue)
                {
                    query = query.Where(p => p.SubCategory.CategoryId == categoryId);
                }
                productsCount = await query.CountAsync();
                if ((page-1) * pageSize >= productsCount)
                {
                    return Enumerable.Empty<Product>();
                }
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
                products = await query.ToListAsync();

                return products;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Product>();
            }
        }

        public async Task<int> Update(Product product)
        {
            try
            {
                dbContext.Products.Update(product);
                await dbContext.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while updating the product. Please try again later.", ex);
            }
        }
    }
}
