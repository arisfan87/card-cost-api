using System.Net;
using CardCostApi.Tests.Integration.TestContainers.Contracts;
using Newtonsoft.Json;

namespace CardCostApi.Tests.Integration.TestContainers
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
            var sut = await _httpClient.GetAsync("/api/card-config/gr");
            var cardConfig =
                JsonConvert.DeserializeObject<CardCostConfig.Response>(await sut.Content.ReadAsStringAsync());

            // assert
            sut.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, sut.StatusCode);
            Assert.Equal(10, cardConfig.Cost);
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
            Assert.Equal(2, cardConfigs.Count());
        }

        [Fact]
        public async Task UpdateCardCost_ValidRequest_204NoContent()
        {
            // act, arrange
            var sut = await _httpClient.PutAsJsonAsync(
                "/api/card-config",
                new CardCostConfig.Request
                {
                    Country = "GR",
                    Cost = 12
                });

            // assert
            sut.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, sut.StatusCode);
        }

        [Fact]
        public async Task UpdateCardCost_InvalidRequest_404NotFound()
        {
            // act, arrange
            var sut = await _httpClient.PutAsJsonAsync(
                "/api/card-config",
                new CardCostConfig.Request
                {
                    Country = "ES",
                    Cost = 12
                });

            // assert
            Assert.Equal(HttpStatusCode.NotFound, sut.StatusCode);
        }

        [Fact]
        public async Task DeleteCardCost_ValidRequest_204NoContent()
        {
            // act, arrange
            var sut = await _httpClient.DeleteAsync("/api/card-config/gr");

            // assert
            Assert.Equal(HttpStatusCode.NoContent, sut.StatusCode);
        }
    }
}