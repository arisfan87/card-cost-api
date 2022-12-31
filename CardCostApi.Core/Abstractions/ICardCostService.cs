using System.Threading.Tasks;

namespace CardCostApi.Core.Abstractions
{
    public interface ICardCostService
    {
        Task<(decimal cost, string country)> GetCardCost(string bin);
    }
}