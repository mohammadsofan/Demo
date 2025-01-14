using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface ICouponRepository
    {
        Task<IEnumerable<Coupon?>> GetAll();
        Task<Coupon?> Get(Guid id);
        Task<int> Create(Coupon coupon);
        Task<int> Update(Coupon coupon);
        Task<Guid> Delete(Coupon coupon);
    }
}
