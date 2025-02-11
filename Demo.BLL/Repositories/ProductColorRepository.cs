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
    public class ProductColorRepository : IProductColorRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProductColorRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<ProductColor>?> GetByProductId(Guid id)
        {
            try
            {

               var result= await dbContext.ProductColors.Where(pc=>pc.ProductId == id).ToListAsync();
               return result;

            }catch(Exception ex)
            {
                throw new InvalidOperationException("An error occurred while getting the productColor. Please try again later.", ex);

            }
        }
        public async Task<int> Create(ProductColor productColor)
        {
            try
            {
                await dbContext.ProductColors.AddAsync(productColor);
                await dbContext.SaveChangesAsync();
                return 1;
            }
            catch (DbUpdateException)
            {
                return 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while creating the productColor. Please try again later.", ex);
            }


        }

        public async Task<int> Delete(ProductColor productColor)
        {
            try
            {
                dbContext.ProductColors.Remove(productColor);
                await dbContext.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while Deleting the productColor. Please try again later.", ex);
            }
        }
        public async Task<int> Update(ProductColor productColor)
        {
            try
            {
                dbContext.ProductColors.Update(productColor);
                await dbContext.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while Updating the productColor. Please try again later.", ex);
            }
        }
    }
}
