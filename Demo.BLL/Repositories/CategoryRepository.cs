using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class CategoryRepository : ICategoryRespository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> Create(Category category)
        {
            try
            {
                await dbContext.Categories.AddAsync(category);
                await dbContext.SaveChangesAsync();    
                return 1;
            }
            catch (DbUpdateException)
            {
                return 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while createing the category. Please try again later.", ex);
            }
            
            
        }

        public async Task<Guid> Delete(Category category)
        {
            try
            {
                dbContext.Categories.Remove(category);
                await dbContext.SaveChangesAsync();
                return category.Id;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while Deleting the category. Please try again later.", ex);
            }
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            try
            {
                return await dbContext.Categories.AsNoTracking().ToListAsync();
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the categories. Please try again later.", ex);
            }
        }
        public async Task<Category> Get(Guid id)
        {
            try
            {
                var result = await dbContext.Categories.FindAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the category. Please try again later.", ex);
            }
        }

        public async Task<int> Update(Category category)
        {
            try
            {
                dbContext.Categories.Update(category);
                await  dbContext.SaveChangesAsync();
                return 1;
            }
            catch (DbUpdateException)
            {
                return 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while createing the category. Please try again later.", ex);
            }
        }

        public async Task<Category> Details(Guid id)
        {
            try
            {
                var category = await dbContext.Categories.Include(c=>c.SubCategories).FirstAsync(c=>c.Id==id);
                return category;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while createing the category. Please try again later.", ex);
            }
        }
    }
}
