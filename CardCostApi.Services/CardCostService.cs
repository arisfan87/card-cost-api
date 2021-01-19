using System.Threading.Tasks;
using CardCostApi.Infrastructure;
using CardCostApi.Infrastructure.Entities;
using CardCostApi.Infrastructure.Store;
using Microsoft.Extensions.Options;

namespace CardCostApi.Services
{
    public class CardCostService : ICardCostService
    {
        private readonly ΙBinListService _binListService;
        private readonly IRepository _repository;
        private readonly IOptions<DefaultCardCostSettings> _options;

        public CardCostService(ΙBinListService binListService, IRepository repository,
            IOptions<DefaultCardCostSettings> options)
        {
            _binListService = binListService;
            _repository = repository;
            _options = options;
        }

        public async Task<(decimal cost, string country)> GetCardCost(string bin)
        {
            var country = await _binListService.GetCountryByCardBin(bin);

            var configuredCardCost = await _repository.GetByIdAsync<CardCostEntity>(country);

            if (configuredCardCost is null)
            {
                return (_options.Value.Cost, _options.Value.Country);
            }

            return (configuredCardCost.Cost, configuredCardCost.Country);
        }
    }
}