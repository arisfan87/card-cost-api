using System.Net;
using CardCostApi.Core.Abstractions;
using CardCostApi.Core.Exceptions;
using CardCostApi.Web.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;

namespace CardCostApi.Tests.Integration.TestContainers
{
    public class CardCostControllerTests : IClassFixture<CardCostWebApplicationFactory>
    {
        private readonly CardCostWebApplicationFactory _factory;
        private HttpClient _httpClient;
        public CardCostControllerTests(CardCostWebApplicationFactory factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
        }

        [Fact]
        public async Task GetCardCost_ValidRequest_200OK()
        {
            // arrange
            var binListService = Mock.Of<ΙBinListService>();
            Mock.Get(binListService)
                .Setup(x => x.GetCountryByCardBin(It.IsAny<string>()))
                .ReturnsAsync("US");

            // act
            var sut = await _httpClient.GetAsync("/api/card-cost/424242");
            sut.EnsureSuccessStatusCode();

            var cardCost = JsonConvert.DeserializeObject<CardCost.Response>(await sut.Content.ReadAsStringAsync());

            // assert
            Assert.Equal("US", cardCost.Country);
            Assert.Equal(15, cardCost.Cost);
            Assert.Equal(HttpStatusCode.OK, sut.StatusCode);
        }

        [Fact]
        public async Task GetCardCost_TooManyRequests_429()
        {
            // arrange
            var binListService = Mock.Of<ΙBinListService>();
            Mock.Get(binListService)
                .Setup(x => x.GetCountryByCardBin(It.IsAny<string>()))
                .ThrowsAsync(new ExternalServiceCommunicationException(string.Empty, HttpStatusCode.TooManyRequests));

            _httpClient = _factory.WithWebHostBuilder(
                    builder =>
                    {
                        builder.ConfigureServices(
                            services =>
                            {
                                services.AddTransient(
                                    provider => binListService);
                            });
                    })
                .CreateClient(new WebApplicationFactoryClientOptions());

            // act
            var sut = await _httpClient.GetAsync("/api/card-cost/424242");

            // assert
            Assert.Equal(HttpStatusCode.TooManyRequests, sut.StatusCode);
        }

        [Fact]
        public async Task GetCardCost_CardBinNotFound_404NotFound()
        {
            // arrange
            var binListService = Mock.Of<ΙBinListService>();
            Mock.Get(binListService)
                .Setup(x => x.GetCountryByCardBin(It.IsAny<string>()))
                .ThrowsAsync(new ExternalServiceCommunicationException(string.Empty, HttpStatusCode.NotFound));

            _httpClient = _factory.WithWebHostBuilder(
                    builder =>
                    {
                        builder.ConfigureServices(
                            services =>
                            {
                                services.AddTransient(
                                    provider => binListService);
                            });
                    })
                .CreateClient(new WebApplicationFactoryClientOptions());

            // act
            var sut = await _httpClient.GetAsync("/api/card-cost/424242");

            // assert
            Assert.Equal(HttpStatusCode.NotFound, sut.StatusCode);
        }
    }
}
