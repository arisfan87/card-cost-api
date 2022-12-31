using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCostApi.Core.Abstractions;
using CardCostApi.Core.Exceptions;
using CardCostApi.Core.Models;

namespace CardCostApi.Core
{
    public class CardCostConfigurationService : ICardCostConfigurationService
    {
        private readonly ICardCostConfigurationRepository _cardCostConfigurationRepository;

        public CardCostConfigurationService(ICardCostConfigurationRepository repository)
        {
            _cardCostConfigurationRepository = repository;
        }

        public async Task Add(CardCost cardCost)
        {
            var cardCostEntity = await _cardCostConfigurationRepository.GetByCountryAsync(cardCost.Country);

            if (cardCostEntity != null)
                throw new CardCostAlreadyExistsException(
                    $"Card cost with key {cardCost.Country} already configured.");

            await _cardCostConfigurationRepository.AddAsync(cardCost);
        }

        public async Task Delete(string country)
        {
            var cardCostEntity = await _cardCostConfigurationRepository.GetByCountryAsync(country);

            if (cardCostEntity is null)
                throw new CardCostNotConfiguredException($"Card cost with key {country} not configured.");

            await _cardCostConfigurationRepository.DeleteAsync(
                new CardCost
                {
                    Cost = cardCostEntity.Cost,
                    Country = cardCostEntity.Country
                });
        }

        public async Task Update(CardCost cardCost)
        {
            var cardCostEntity = await _cardCostConfigurationRepository.GetByCountryAsync(cardCost.Country);

            if (cardCostEntity is null)
                throw new CardCostNotConfiguredException($"Card cost with key {cardCost.Country} not configured.");

            await _cardCostConfigurationRepository.UpdateAsync(
                new CardCost
                {
                    Cost = cardCost.Cost,
                    Country = cardCost.Country
                });
        }

        public async Task<CardCost> GetByCountry(string country)
        {
            var cardCostEntity = await _cardCostConfigurationRepository.GetByCountryAsync(country);

            if (cardCostEntity is null)
                throw new CardCostNotConfiguredException($"Card cost with key {country} not configured.");

            return new CardCost
            {
                Cost = cardCostEntity.Cost,
                Country = cardCostEntity.Country
            };
        }

        public async Task<IEnumerable<CardCost>> GetAll()
        {
            var cardCostEntities = await _cardCostConfigurationRepository.ListAsync();

            return cardCostEntities.Select(
                s => new CardCost
                {
                    Cost = s.Cost,
                    Country = s.Country
                });
        }
    }
}