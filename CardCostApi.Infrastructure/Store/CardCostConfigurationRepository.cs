using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<CardCostEntity> GetByCountryAsync(string id)
        {
            return await _dbContext.CardCosts.FirstOrDefaultAsync(x => x.Country.Equals(id));
        }

        public async Task<List<CardCostEntity>> ListAsync()
        {
            return await _dbContext.CardCosts.ToListAsync();
        }

        public async Task AddAsync(CardCostEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Added;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(CardCostEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(CardCostEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
        }
    }
}