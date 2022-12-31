using System.Collections.Generic;
using System.Threading.Tasks;
using CardCostApi.Core.Models;

namespace CardCostApi.Core.Abstractions
{
    public interface ICardCostConfigurationService
    {
        Task Add(CardCost cardCost);
        Task Delete(string country);
        Task Update(CardCost cardCostDto);
        Task<CardCost> GetByCountry(string country);
        Task<IEnumerable<CardCost>> GetAll();
    }
}