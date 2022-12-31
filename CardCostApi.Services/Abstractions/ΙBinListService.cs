using System.Threading.Tasks;

namespace CardCostApi.Core.Abstractions
{
    public interface ΙBinListService
    {
        Task<string> GetCountryByCardBin(string bin);
    }
}