using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
     public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetByCategory(Guid categoryId);
        Task<Product?> Get(Guid id);
        Task<int> Create(Product product);
        Task<int> Update(Product product);
        Task<Guid> Delete(Product product);
        Task<Product> Details(Guid id);
    }
}
