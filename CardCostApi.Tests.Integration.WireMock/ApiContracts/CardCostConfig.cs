namespace CardCostApi.Tests.Integration.WireMock.ApiContracts
{
    public class CardCostConfig
    {
        public class Request
        {
            public string Country { get; set; }
            public decimal? Cost { get; set; } = null!;
        }

        public class Response
        {
            public string Country { get; set; }
            public decimal Cost { get; set; }
        }
    }
}