using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CardCostApi.Core.Abstractions;
using CardCostApi.Core.Exceptions;

namespace CardCostApi.Infrastructure.BinList
{
    public class BinListService : ΙBinListService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public BinListService(IHttpClientFactory client)
        {
            _clientFactory = client;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
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

            await using var content = await response.Content.ReadAsStreamAsync();
            var cardMetadata = await JsonSerializer.DeserializeAsync<CardMetadata>(
                content, _jsonSerializerOptions);

            if (cardMetadata is null)
                throw new ArgumentNullException(nameof(cardMetadata), "Card metadata is null.");

            return cardMetadata.Country.Alpha2;
        }
    }
}
