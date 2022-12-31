using System.Threading.Tasks;
using CardCostApi.Core.Abstractions;
using CardCostApi.Core.Settings;
using Microsoft.Extensions.Options;

namespace CardCostApi.Core
{
    public class CardCostService : ICardCostService
    {
        private readonly ΙBinListService _binListService;
        private readonly ICardCostConfigurationRepository _cardCostConfigurationRepository;
        private readonly IOptions<DefaultCardCostSettings> _options;

        public CardCostService(ΙBinListService binListService, ICardCostConfigurationRepository repository,
            IOptions<DefaultCardCostSettings> options)
        {
            _binListService = binListService;
            _cardCostConfigurationRepository = repository;
            _options = options;
        }

        public async Task<(decimal cost, string country)> GetCardCost(string bin)
        {
            var country = await _binListService.GetCountryByCardBin(bin);

            var configuredCardCost = await _cardCostConfigurationRepository.GetByCountryAsync(country);

            if (configuredCardCost is null)
            {
                return (_options.Value.Cost, _options.Value.Country);
            }

            return (configuredCardCost.Cost, configuredCardCost.Country);
        }
    }
}