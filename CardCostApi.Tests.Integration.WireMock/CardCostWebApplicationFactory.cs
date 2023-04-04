using CardCostApi.Infrastructure.Store;
using CardCostApi.Web;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WireMock.Server;

namespace CardCostApi.Tests.Integration.WireMock
{
    public class CardCostWebApplicationFactory : WebApplicationFactory<Startup>, IAsyncLifetime
    {
        protected internal readonly TestcontainerDatabase _dbContainer = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "cardcosts",
                Username = "postgres",
                Password = "postgres",
            })
            .WithAutoRemove(false)
            .WithImage("postgres:14.4-alpine")
            .WithName($"{nameof(CardCostWebApplicationFactory)}{nameof(PostgreSqlTestcontainer)}{Guid.NewGuid():N}")
            .WithCleanUp(true)
            .Build();

        protected internal readonly WireMockServer _binListVirtualServer = WireMockServer.Start();
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new KeyValuePair<string, string?>[]
                {
                    new("BinListBaseUrl", _binListVirtualServer.Urls.First()),
                    new("DefaultCardCostSettings:Country:Other:Cost", "10")
                }).Build();
            });

            builder.UseEnvironment("Testing");

            builder.ConfigureTestServices(
                services =>
                {
                    services.RemoveAll(typeof(DbConfiguration));

                    services.Configure<DbConfiguration>(config =>
                    {
                        config.ConnectionString = _dbContainer.ConnectionString;
                    });
                });
        }
        
        public async Task InitializeAsync()
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            await _dbContainer.StartAsync(cts.Token);
            await _dbContainer.ExecScriptAsync(Migrations.InitialDbSchema, CancellationToken.None);
        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();

            _binListVirtualServer.Stop();
            _binListVirtualServer.Dispose();
        }
    }
}


















//private void InitialisePostgreSqlDb()
//{
//    using var conn = new NpgsqlConnection(_container.ConnectionString);
//    var script = Migrations.InitialDbSchema;
//    using var command = new NpgsqlCommand(script, conn);
//    conn.Open();
//    command.ExecuteNonQuery();
//    conn.Close();
//}
