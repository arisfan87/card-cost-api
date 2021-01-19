using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using CardCostApi.Infrastructure;
using CardCostApi.Services;
using CardCostApi.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CardCostApi.Web.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    [ApiController]
    public class CardCostController : ControllerBase
    {
        private readonly ICardCostService _cardCostService;

        public CardCostController(ICardCostService cardCostService)
        {
            _cardCostService = cardCostService;
        }

        [HttpGet("card-cost/{bin}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> GetCardCost([FromRoute] [MinLength(6)] [MaxLength(6)]
            string bin)
        {
            try
            {
                var (cost, country) = await _cardCostService.GetCardCost(bin);

                return Ok(
                    new CardCost.Response
                    {
                        Cost = cost,
                        Country = country
                    });
            }
            catch (ExternalServiceCommunicationException e) when (e.StatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest();
            }
            catch (ExternalServiceCommunicationException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            catch (ExternalServiceCommunicationException e) when(e.StatusCode == HttpStatusCode.TooManyRequests)
            {
                return StatusCode(429);
            }
        }
    }
}