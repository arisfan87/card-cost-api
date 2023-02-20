using System.Net;
using CardCostApi.Tests.Integration.WireMock.ApiContracts;
using Newtonsoft.Json;

namespace CardCostApi.Tests.Integration.WireMock
{
    public class CardCostConfigurationControllerTests : IClassFixture<CardCostWebApplicationFactory>
    {
        private readonly CardCostWebApplicationFactory _factory;
        private readonly HttpClient _httpClient;
        public CardCostConfigurationControllerTests(CardCostWebApplicationFactory factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
        }

        [Fact]
        public async Task CreateCardCost_ValidRequest_204NoContent()
        {
            // act, arrange
            var sut = await _httpClient.PostAsJsonAsync(
                "/api/card-config",
                new CardCostConfig.Request
                {
                    Country = "ES", Cost = 9
                });

            // assert
            sut.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, sut.StatusCode);
        }

        [Fact]
        public async Task CreateCardCost_InvalidRequest_400BadRequest()
        {
            // act, arrange
            var sut = await _httpClient.PostAsJsonAsync(
                "/api/card-config",
                new CardCostConfig.Request
                {
                    Country = "ESP",
                    Cost = 9
                });

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, sut.StatusCode);
        }

        [Fact]
        public async Task GetCardCost_ValidRequest_200OK()
        {
            // act, arrange
            var sut = await _httpClient.GetAsync("/api/card-config/us");
            var cardConfig =
                JsonConvert.DeserializeObject<CardCostConfig.Response>(await sut.Content.ReadAsStringAsync());

            // assert
            sut.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, sut.StatusCode);
            Assert.Equal(15, cardConfig.Cost);
        }

        [Fact]
        public async Task GetAllCardCosts_ValidRequest_200OK()
        {
            // act, arrange
            var sut = await _httpClient.GetAsync("/api/card-config");
            var cardConfigs =
                JsonConvert.DeserializeObject<IEnumerable<CardCostConfig.Response>>(
                    await sut.Content.ReadAsStringAsync());

            // assert
            sut.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, sut.StatusCode);
            Assert.NotEmpty(cardConfigs);
        }

        [Fact]
        public async Task UpdateCardCost_InvalidRequest_404NotFound()
        {
            // act, arrange
            var sut = await _httpClient.PutAsJsonAsync(
                "/api/card-config",
                new CardCostConfig.Request
                {
                    Country = "CY",
                    Cost = 12
                });

            // assert
            Assert.Equal(HttpStatusCode.NotFound, sut.StatusCode);
        }
    }
}