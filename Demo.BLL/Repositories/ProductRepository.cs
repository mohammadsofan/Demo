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
