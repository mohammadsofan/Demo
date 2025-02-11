using Demo.DAL.Enums;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface ICardRepository
    {
        Task<IEnumerable<Card>> GetAllSectionA();
        Task<IEnumerable<Card>> GetAllSectionB();
        Task<IEnumerable<Card>> GetAll();
        Task<bool> IsExistedOrderSectionA(int order);
        Task<bool> IsExistedOrderSectionB(int order);
        Task<Card?> Get(Guid id);
        Task<int> GetSectionACount();
        Task<int> GetSectionBCount();
        Task<int> Create(Card card);
        Task<int> Update(Card card);
        Task<int> Delete(Card card);
    }
}
