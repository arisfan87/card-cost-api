using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCostApi.Core.Abstractions;
using CardCostApi.Core.Models;
using CardCostApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CardCostApi.Infrastructure.Store
{
    public class CardCostConfigurationRepository : ICardCostConfigurationRepository
    {
        private readonly CardCostContext _dbContext;

        public CardCostConfigurationRepository(CardCostContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CardCost> GetByCountryAsync(string id)
        {
            var cardCostEntity = await _dbContext.CardCosts.FirstOrDefaultAsync(x => x.Country.Equals(id));

            if (cardCostEntity == null) return null;

            return new CardCost
            {
                Cost = cardCostEntity.Cost,
                Country = cardCostEntity.Country
            };
        }

        public async Task<List<CardCost>> ListAsync()
        {
            var cardsCost = await _dbContext.CardCosts.ToListAsync();

            return cardsCost.Select(s => new CardCost
            {
                Cost = s.Cost,
                Country = s.Country
            }).ToList();
        }

        public async Task AddAsync(CardCost cardCost)
        {
            var entity = new CardCostEntity
            {
                Cost = cardCost.Cost,
                Country = cardCost.Country
            };

            _dbContext.Entry(entity).State = EntityState.Added;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(CardCost cardCost)
        {
            var entity = new CardCostEntity
            {
                Cost = cardCost.Cost,
                Country = cardCost.Country
            };

            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(CardCost cardCost)
        {
            var entity = new CardCostEntity
            {
                Cost = cardCost.Cost,
                Country = cardCost.Country
            };

            _dbContext.Entry(entity).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
        }
    }
}