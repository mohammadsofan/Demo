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
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CouponRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> Create(Coupon coupon)
        {
            try
            {
                await dbContext.Coupons.AddAsync(coupon);
                await dbContext.SaveChangesAsync();
                return 1;

            }
            catch (DbUpdateException)
            {
                return 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while createing the coupon. Please try again later.", ex);
            }
        }

        public async Task<Guid> Delete(Coupon coupon)
        {
            try
            {
                dbContext.Coupons.Remove(coupon);
                await dbContext.SaveChangesAsync();
                return coupon.Id;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while deleting the coupon. Please try again later.", ex);
            }
        }

        public async Task<Coupon?> Get(Guid id)
        {
            try
            {
                var coupon = await dbContext.Coupons.FindAsync(id);
                return coupon;

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the coupon. Please try again later.", ex);
            }
        }

        public async Task<IEnumerable<Coupon?>> GetAll()
        {
            try
            {
                return await dbContext.Coupons.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the coupons. Please try again later.", ex);
            }
        }

        public async Task<int> Update(Coupon coupon)
        {
            try
            {
                dbContext.Coupons.Update(coupon);
                await dbContext.SaveChangesAsync();
                return 1;
            }
            catch (DbUpdateException)
            {
                return 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while updating the coupon. Please try again later.", ex);
            }
        }
    }
}
