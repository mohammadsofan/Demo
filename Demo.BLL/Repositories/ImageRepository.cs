using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Demo.BLL.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ImageRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<DAL.Models.Image?> Get(Guid id)
        {
            try
            {
                var image = await dbContext.Images.FindAsync(id);
                return image;

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the image. Please try again later.", ex);
            }
        }
        public async Task<int> Create(DAL.Models.Image image)
        {
            try
            {
                await dbContext.Images.AddAsync(image);
                await dbContext.SaveChangesAsync();
                return 1;
            }
            catch (DbUpdateException)
            {
                return 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while creating the image. Please try again later.", ex);
            }


        }

        public async Task<int> Delete(DAL.Models.Image image)
        {
            try
            {
                dbContext.Images.Remove(image);
                await dbContext.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while Deleting the image. Please try again later.", ex);
            }
        }
        public async Task<int> Update(DAL.Models.Image image)
        {
            try
            {
                dbContext.Images.Update(image);
                await dbContext.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while updating the image. Please try again later.", ex);
            }
        }
    }
}
