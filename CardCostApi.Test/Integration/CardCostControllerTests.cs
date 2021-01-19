﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CardCostApi.Infrastructure;
using CardCostApi.Services;
using CardCostApi.Web.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace CardCostApi.Test.Integration
{
    public class CardCostControllerTests : IntegrationBaseTest
    {
        private HttpClient _httpClient;

        public CardCostControllerTests(ApiWebApplicationFactory fixture)
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
        public async Task GetCardCost_ValidRequest_200OK()
        {
            // act, arrange
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