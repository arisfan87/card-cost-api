using System.Threading.Tasks;

namespace CardCostApi.Services
{
    public interface ΙBinListService
    {
        Task<string> GetCountryByCardBin(string bin);
    }
}