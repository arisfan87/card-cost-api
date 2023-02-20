namespace CardCostApi.Tests.Integration.WireMock.ApiContracts
{
    public class CardCost
    {
        public class Request
        {
            public string Bin { get; set; }
        }

        public class Response
        {
            public string Country { get; set; }
            public decimal Cost { get; set; }
        }
    }
}