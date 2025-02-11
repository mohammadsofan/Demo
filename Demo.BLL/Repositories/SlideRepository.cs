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
    public class SlideRepository:ISlideRepository
    {
        private readonly ApplicationDbContext dbContext;

        public SlideRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> Create(Slide slide)
        {
            try
            {
                await dbContext.Slides.AddAsync(slide);
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

        public async Task<Guid> Delete(Slide slide)
        {
            try
            {
                dbContext.Slides.Remove(slide);
                await dbContext.SaveChangesAsync();
                return slide.Id;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while Deleting the category. Please try again later.", ex);
            }
        }

        public async Task<IEnumerable<Slide?>> GetAll()
        {
            try
            {
                return await dbContext.Slides.AsNoTracking().OrderBy(s=>s.Order).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the categories. Please try again later.", ex);
            }
        }
        public async Task<Slide?> Get(Guid id)
        {
            try
            {
                var result = await dbContext.Slides.FindAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the category. Please try again later.", ex);
            }
        }

        public async Task<int> Update(Slide slide)
        {
            try
            {
                dbContext.Slides.Update(slide);
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
    }
}
