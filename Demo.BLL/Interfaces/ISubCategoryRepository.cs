using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface ISubCategoryRepository
    {
        Task<IEnumerable<SubCategory?>> GetAll();
        Task<IEnumerable<SubCategory?>> GetAllWithCategory();
        Task<SubCategory?> Get(Guid id);
        Task<int> Create(SubCategory category);
        Task<int> Update(SubCategory category);
        Task<Guid> Delete(SubCategory category);
    }
}
