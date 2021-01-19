using System.Collections.Generic;
using System.Threading.Tasks;
using CardCostApi.Infrastructure.Dtos;

namespace CardCostApi.Services
{
    public interface ICardCostConfigurationService
    {
        Task Add(CardCostDto cardCost);
        Task Delete(string country);
        Task Update(CardCostDto cardCostDto);
        Task<CardCostDto> Get(string country);
        Task<IEnumerable<CardCostDto>> GetAll();
    }
}