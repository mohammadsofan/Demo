using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface ISlideRepository
    {
        Task<IEnumerable<Slide?>> GetAll();
        Task<Slide?> Get(Guid id);
        Task<int> Create(Slide slide);
        Task<int> Update(Slide slide);
        Task<Guid> Delete(Slide slide);
    }
}
