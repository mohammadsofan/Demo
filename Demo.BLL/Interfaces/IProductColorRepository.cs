using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IProductColorRepository
    {
        Task<List<ProductColor>?> GetByProductId(Guid id);
        Task<int> Create(ProductColor productColor);
        Task<int> Update(ProductColor productColor);
        Task<int> Delete(ProductColor productColor);
    }
}
