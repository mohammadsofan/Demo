using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category?>> GetAll();
        Task<IEnumerable<Category>> GetPopular();
        Task<Category?> Get(Guid id);
        Task<int> Create(Category category);
        Task<int> Update(Category category);
        Task<Guid> Delete(Category category);
        Task<Category> Details(Guid id);


    }
}
