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
    public class SubCategoryRepository:ISubCategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public SubCategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> Create(SubCategory subCategory)
        {
            try
            {
                await dbContext.SubCategories.AddAsync(subCategory);
                await dbContext.SaveChangesAsync();
                return 1;
            }
            catch (DbUpdateException)
            {
                return 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while createing the subCategory. Please try again later.", ex);
            }


        }

        public async Task<Guid> Delete(SubCategory subCategory)
        {
            try
            {
                dbContext.SubCategories.Remove(subCategory);
                await dbContext.SaveChangesAsync();
                return subCategory.Id;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while Deleting the subCategory. Please try again later.", ex);
            }
        }

        public async Task<IEnumerable<SubCategory?>> GetAll()
        {
            try
            {
                return await dbContext.SubCategories.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the subCategories. Please try again later.", ex);
            }
        }
        public async Task<IEnumerable<SubCategory?>> GetAllWithCategory()
        {
            try
            {
                return await dbContext.SubCategories.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the subCategories. Please try again later.", ex);
            }
        }
        public async Task<SubCategory?> Get(Guid id)
        {
            try
            {
                var result = await dbContext.SubCategories.FindAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the subCategory. Please try again later.", ex);
            }
        }

        public async Task<int> Update(SubCategory subCategory)
        {
            try
            {
                dbContext.SubCategories.Update(subCategory);
                await dbContext.SaveChangesAsync();
                return 1;
            }
            catch (DbUpdateException)
            {
                return 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while createing the subCategory. Please try again later.", ex);
            }
        }
    }
}

