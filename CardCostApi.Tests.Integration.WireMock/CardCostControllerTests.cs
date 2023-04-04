using System.Net;
using CardCostApi.Tests.Integration.WireMock.ApiContracts;
using CardCostApi.Tests.Integration.WireMock.BrokerContracts;
using Newtonsoft.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace CardCostApi.Tests.Integration.WireMock
{
    public class CardCostControllerTests : IClassFixture<CardCostWebApplicationFactory>
    {
        private readonly CardCostWebApplicationFactory _factory;
        private readonly HttpClient _httpClient;
        
        public CardCostControllerTests(CardCostWebApplicationFactory factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
        }

        [Fact]
        public async Task GetCardCost_ValidRequest_200OK()
        {
            // arrange
            var bin = "424242";

            _factory._binListVirtualServer
                .Given(Request.Create().WithPath($"/{bin}").UsingGet())
                .RespondWith(Response.Create().WithBodyAsJson(new CardMetadata {
                        Country = new Country
                        {
                            Alpha2 = "US",
                            Currency = "USD"
                        }}).WithStatusCode(HttpStatusCode.OK));

            // act
            var sut = await _httpClient.GetAsync($"/api/card-cost/{bin}");
            sut.EnsureSuccessStatusCode();

            var cardCost = JsonConvert.DeserializeObject<CardCost.Response>(await sut.Content.ReadAsStringAsync());
            
            // assert
            Assert.Equal("US", cardCost.Country);
            Assert.Equal(15, cardCost.Cost);
            Assert.Equal(HttpStatusCode.OK, sut.StatusCode);
        }

        [Fact(DisplayName = "This test will fail. Its an edge case of malformed response. It is not handled in the code.")]
        public async Task GetCardCost_MalformedResponse_()
        {
            // arrange
            var bin = "934567";
            _factory._binListVirtualServer
                .Given(Request.Create().WithPath($"/{bin}").UsingGet())
                .RespondWith(Response.Create()
                        .WithFault(FaultType.MALFORMED_RESPONSE_CHUNK)
                        .WithStatusCode(HttpStatusCode.OK));

            // act
            var sut = await _httpClient.GetAsync($"/api/card-cost/{bin}");

            // assert
            Assert.Equal(HttpStatusCode.OK, sut.StatusCode);
        }
    }
}
