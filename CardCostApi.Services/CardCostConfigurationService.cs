using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCostApi.Infrastructure;
using CardCostApi.Infrastructure.Dtos;
using CardCostApi.Infrastructure.Entities;
using CardCostApi.Infrastructure.Store;

namespace CardCostApi.Services
{
    public class CardCostConfigurationService : ICardCostConfigurationService
    {
        private readonly IRepository _repository;

        public CardCostConfigurationService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task Add(CardCostDto cardCostDto)
        {
            var cardCost = await _repository.GetByIdAsync<CardCostEntity>(cardCostDto.Country);

            if (cardCost != null) throw new CardCostAlreadyExistsException($"Card cost with key {cardCostDto.Country} already configured.");

            var cardCostEntity = new CardCostEntity
            {
                Cost = cardCostDto.Cost,
                Country = cardCostDto.Country
            };

            await _repository.AddAsync(cardCostEntity);
        }

        public async Task Delete(string country)
        {
            var cardCost = await _repository.GetByIdAsync<CardCostEntity>(country);

            if (cardCost is null) throw new CardCostNotConfiguredException($"Card cost with key {country} not configured.");

            var cardCostEntity = new CardCostEntity
            {
                Cost = cardCost.Cost,
                Country = cardCost.Country
            };

            await _repository.DeleteAsync(cardCostEntity);
        }

        public async Task Update(CardCostDto cardCostDto)
        {
            var cardCost = await _repository.GetByIdAsync<CardCostEntity>(cardCostDto.Country);

            if (cardCost is null) throw new CardCostNotConfiguredException($"Card cost with key {cardCostDto.Country} not configured.");

            var cardCostEntity = new CardCostEntity
            {
                Cost = cardCostDto.Cost,
                Country = cardCostDto.Country
            };

            try
            {
                await _repository.UpdateAsync(cardCostEntity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<CardCostDto> Get(string country)
        {
            var cardCost = await _repository.GetByIdAsync<CardCostEntity>(country);

            if (cardCost is null) throw new CardCostNotConfiguredException($"Card cost with key {country} not configured.");

            return new CardCostDto
            {
                Cost = cardCost.Cost,
                Country = cardCost.Country
            };
        }

        public async Task<IEnumerable<CardCostDto>> GetAll()
        {
            var cardCostEntity = await _repository.ListAsync<CardCostEntity>();

            return cardCostEntity.Select(
                s => new CardCostDto
                {
                    Cost = s.Cost,
                    Country = s.Country
                });
        }
    }
}