using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CardCostApi.Infrastructure;
using CardCostApi.Infrastructure.Exceptions;

namespace CardCostApi.Services
{
    public class BinListService : ΙBinListService
    {
        private IHttpClientFactory _clientFactory { get; }

        public BinListService(IHttpClientFactory client)
        {
            _clientFactory = client;
        }

        public async Task<string> GetCountryByCardBin(string bin)
        {
            var client = _clientFactory.CreateClient("BinListClient");

            var response = await client.SendAsync(
                new HttpRequestMessage(
                    HttpMethod.Get,
                    $"/{bin}"));

            if (!response.IsSuccessStatusCode)
            {
                throw new ExternalServiceCommunicationException(
                    "Response code does not indicate success.",
                    response.StatusCode);
            }

            var cardMetadata = await response.Content.ReadFromJsonAsync<CardMetadata>();

            if (cardMetadata is null)
                throw new ArgumentNullException(nameof(cardMetadata), "Card cost metadata is null.");

            return cardMetadata.Country.Alpha2;
        }
    }
}