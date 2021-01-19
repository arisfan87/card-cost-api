using System.Collections.Generic;
using System.Threading.Tasks;
using CardCostApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CardCostApi.Infrastructure.Store
{
    public class EfRepository : IRepository
    {
        private readonly CardCostContext _dbContext;

        public EfRepository(CardCostContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetByIdAsync<T>(dynamic id) where T : BaseEntity
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> ListAsync<T>() where T : BaseEntity
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task AddAsync<T>(T entity) where T : BaseEntity
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public Task UpdateAsync<T>(T entity) where T : BaseEntity
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return _dbContext.SaveChangesAsync();
        }

        public Task DeleteAsync<T>(T entity) where T : BaseEntity
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
            return _dbContext.SaveChangesAsync();
        }
    }
}