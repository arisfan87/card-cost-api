namespace CardCostApi.Tests.Integration.TestContainers
{
    public class CardCostWebApiSettings
    {
        public Dictionary<string, string> GetSettings =>
            new()
            {
                {"DefaultCardCostSettings:Country:Other:Cost", "10"},
                {"BinListBaseUrl", "https://lookup.binlist.net/"},
            };
    }
}