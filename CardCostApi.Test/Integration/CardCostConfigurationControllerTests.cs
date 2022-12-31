using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CardCostApi.Infrastructure.Store;
using CardCostApi.Web.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace CardCostApi.Test.Integration
{
    public class CardCostConfigurationControllerTests : IntegrationBaseTest
    {
        private readonly HttpClient _httpClient;

        public CardCostConfigurationControllerTests(ApiWebApplicationFactory fixture)
            : base(fixture)
        {
            _httpClient = _factory.WithWebHostBuilder(
                    builder =>
                    {
                        builder.ConfigureServices(
                            services =>
                            {
                                var sp = services.BuildServiceProvider();
                                using var scope = sp.CreateScope();
                                var scopedServices = scope.ServiceProvider;
                                var db = scopedServices.GetRequiredService<CardCostContext>();
                                db.Database.EnsureCreated();
                                ResetDb(db);
                            });
                    })
                .CreateClient(new WebApplicationFactoryClientOptions());
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