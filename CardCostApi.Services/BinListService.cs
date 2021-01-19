using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CardCostApi.Infrastructure;

namespace CardCostApi.Services
{
    public class BinListService : ΙBinListService
    {
        public IHttpClientFactory ClientFactory { get; }

        public BinListService(IHttpClientFactory client)
        {
            ClientFactory = client;
        }

        public async Task<string> GetCountryByCardBin(string bin)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                string.Format($"https://lookup.binlist.net/{bin}", bin));

            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new ExternalServiceCommunicationException(
                    "Response code does not indicate success.",
                    response.StatusCode);
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var jsonString = await response.Content.ReadAsStringAsync();
            var cardMetadata = JsonSerializer.Deserialize<CardMetadata>(jsonString, options);

            return cardMetadata.Country.Alpha2;
        }
    }
}