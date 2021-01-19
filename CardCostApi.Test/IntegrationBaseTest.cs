using CardCostApi.Infrastructure;
using CardCostApi.Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace CardCostApi.Test
{
    public abstract class IntegrationBaseTest : IClassFixture<ApiWebApplicationFactory>
    {
        protected readonly ApiWebApplicationFactory _factory;
        protected readonly IConfiguration _configuration;

        protected IntegrationBaseTest(ApiWebApplicationFactory fixture)
        {
            _factory = fixture;
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("integrationsettings.json")
                .Build();
        }

        protected void ResetDb(CardCostContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var cardCost1 = new CardCostEntity
            {
                Country = "GR",
                Cost = 10
            };

            context.CardCosts.Add(cardCost1);

            var cardCost2 = new CardCostEntity
            {
                Country = "US",
                Cost = 15
            };

            context.CardCosts.Add(cardCost2);

            context.SaveChanges();
        }
    }
}