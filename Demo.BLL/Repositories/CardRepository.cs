using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Enums;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class CardRepository:ICardRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CardRepository(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<Card>> GetAllSectionA()
        {
            try
            {
                var cards = await dbContext.Cards.AsNoTracking().Where(c=>c.Type==CardType.SectionA).ToListAsync();
                return cards;
            }catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
        public async Task<IEnumerable<Card>> GetAllSectionB()
        {
            try
            {
                var cards = await dbContext.Cards.AsNoTracking().Where(c => c.Type == CardType.SectionB).ToListAsync();
                return cards;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
        public async Task<IEnumerable<Card>> GetAll()
        {
            try
            {
                var cards = await dbContext.Cards.AsNoTracking().ToListAsync();
                return cards;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<Card?> Get(Guid id)
        {
            try
            {
                return await dbContext.Cards.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
        public async Task<int> GetSectionACount()
        {
            try
            {
                return await dbContext.Cards.Where(c=>c.Type==CardType.SectionA).CountAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<int> GetSectionBCount()
        {
            try
            {
                return await dbContext.Cards.Where(c => c.Type == CardType.SectionB).CountAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
        public async Task<bool> IsExistedOrderSectionA(int order)
        {
            try
            {
                var orders = await dbContext.Cards.Where(c => c.Type == CardType.SectionA).Select(c => c.Order).ToListAsync();
                if (orders.Contains(order))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<bool> IsExistedOrderSectionB(int order)
        {
            try
            {
                var orders = await dbContext.Cards.Where(c => c.Type == CardType.SectionB).Select(c => c.Order).ToListAsync();
                if (orders.Contains(order))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<int> Create(Card card)
        {
            try
            {
                await dbContext.Cards.AddAsync(card);
                await dbContext.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
        public async Task<int> Update(Card card)
        {
            try
            {
                dbContext.Cards.Update(card);
                await dbContext.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
        public async Task<int> Delete(Card card)
        {
            try
            {
                dbContext.Cards.Remove(card);
                await dbContext.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }


    }
}
