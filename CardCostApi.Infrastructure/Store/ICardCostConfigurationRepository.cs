using System.Collections.Generic;
using System.Threading.Tasks;
using CardCostApi.Infrastructure.Entities;

namespace CardCostApi.Infrastructure.Store
{
    public interface ICardCostConfigurationRepository
    {
        Task<CardCostEntity> GetByCountryAsync(string id);
        Task<List<CardCostEntity>> ListAsync();
        Task AddAsync(CardCostEntity entity);
        Task UpdateAsync(CardCostEntity entity);
        Task DeleteAsync(CardCostEntity entity);
    }
}