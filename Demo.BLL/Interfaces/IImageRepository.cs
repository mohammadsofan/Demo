using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IImageRepository
    {
        Task<int> Create(Demo.DAL.Models.Image image);
        Task<DAL.Models.Image?> Get(Guid id);
        Task<int> Update(Demo.DAL.Models.Image image);
        Task<int> Delete(Demo.DAL.Models.Image image);

    }
}
