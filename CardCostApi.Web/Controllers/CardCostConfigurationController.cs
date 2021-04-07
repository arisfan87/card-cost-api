using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CardCostApi.Infrastructure.Exceptions;
using CardCostApi.Services;
using CardCostApi.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CardCost = CardCostApi.Infrastructure.Dtos.CardCost;

namespace CardCostApi.Web.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    [ApiController]
    public class CardCostConfigurationController : ControllerBase
    {
        private readonly ICardCostConfigurationService _cardCostConfigurationService;

        public CardCostConfigurationController(ICardCostConfigurationService cardCostConfigurationService)
        {
            _cardCostConfigurationService = cardCostConfigurationService;
        }

        [HttpPost("card-config")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CardCostConfig.Request request)
        {
            try
            {
                await _cardCostConfigurationService.Add(
                    new CardCost
                    {
                        Country = request.Country,
                        Cost = request.Cost.Value
                    });

                return NoContent();
            }
            catch (CardCostAlreadyExistsException)
            {
                return Conflict();
            }
        }

        [HttpPut("card-config")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] CardCostConfig.Request request)
        {
            try
            {
                await _cardCostConfigurationService.Update(
                    new CardCost
                    {
                        Country = request.Country,
                        Cost = request.Cost.Value
                    });

                return NoContent();
            }
            catch (CardCostNotConfiguredException)
            {
                return NotFound();
            }
        }

        [HttpGet("card-config/{country}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] [MinLength(2)] [MaxLength(2)]
            string country)
        {
            try
            {
                var result = await _cardCostConfigurationService.GetByCountry(country.ToUpper());

                return Ok(
                    new CardCostConfig.Response
                    {
                        Cost = result.Cost,
                        Country = result.Country
                    });
            }
            catch (CardCostNotConfiguredException)
            {
                return NotFound();
            }
        }

        [HttpGet("card-config")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _cardCostConfigurationService.GetAll();

            return Ok(
                result.Select(
                    x => new CardCostConfig.Response
                    {
                        Cost = x.Cost,
                        Country = x.Country
                    }));
        }

        [HttpDelete("card-config/{country}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] [MinLength(2)] [MaxLength(2)]
            string country)
        {
            try
            {
                await _cardCostConfigurationService.Delete(country.ToUpper());

                return NoContent();
            }
            catch (CardCostNotConfiguredException)
            {
                return NotFound();
            }
        }
    }
}