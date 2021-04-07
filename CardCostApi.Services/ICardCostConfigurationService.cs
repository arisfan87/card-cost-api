using System.Collections.Generic;
using System.Threading.Tasks;
using CardCostApi.Infrastructure.Dtos;

namespace CardCostApi.Services
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