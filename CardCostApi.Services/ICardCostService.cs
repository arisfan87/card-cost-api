using System.Threading.Tasks;

namespace CardCostApi.Services
{
    public interface ICardCostService
    {
        Task<(decimal cost, string country)> GetCardCost(string bin);
    }
}