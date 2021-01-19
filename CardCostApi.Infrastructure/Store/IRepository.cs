using System.Collections.Generic;
using System.Threading.Tasks;
using CardCostApi.Infrastructure.Entities;

namespace CardCostApi.Infrastructure.Store
{
    public interface IRepository
    {
        Task<T> GetByIdAsync<T>(dynamic id) where T : BaseEntity;

        Task<List<T>> ListAsync<T>() where T : BaseEntity;
        Task AddAsync<T>(T entity) where T : BaseEntity;
        Task UpdateAsync<T>(T entity) where T : BaseEntity;
        Task DeleteAsync<T>(T entity) where T : BaseEntity;
    }
}