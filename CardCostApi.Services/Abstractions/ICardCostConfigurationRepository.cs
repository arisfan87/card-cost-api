using System.Collections.Generic;
using System.Threading.Tasks;
using CardCostApi.Core.Models;

namespace CardCostApi.Core.Abstractions
{
    public interface ICardCostConfigurationRepository
    {
        Task<CardCost> GetByCountryAsync(string id);
        Task<List<CardCost>> ListAsync();
        Task AddAsync(CardCost cardCost);
        Task UpdateAsync(CardCost cardCost);
        Task DeleteAsync(CardCost cardCost);
    }
}