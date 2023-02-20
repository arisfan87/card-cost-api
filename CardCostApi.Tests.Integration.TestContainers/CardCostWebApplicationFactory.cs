using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using CardCostApi.Core.Abstractions;
using CardCostApi.Infrastructure.BinList;
using CardCostApi.Infrastructure.Entities;
using CardCostApi.Infrastructure.Store;
using CardCostApi.Web;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Humanizer.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace CardCostApi.Tests.Integration.TestContainers
{
    public class CardCostWebApplicationFactory : WebApplicationFactory<Startup>, IAsyncLifetime
    {
        private readonly TestcontainerDatabase _container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "cardcosts",
                Username = "postgres",
                Password = "postgres",
            })
            .WithAutoRemove(true)
            .WithImage("postgres:14.4-alpine")
            .WithName($"{nameof(CardCostWebApplicationFactory)}{nameof(PostgreSqlTestcontainer)}{Guid.NewGuid():N}")
            .WithCleanUp(true)
            .Build();

        private readonly Dictionary<string, string> _settings = new CardCostWebApiSettings().GetSettings;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config
                    .AddInMemoryCollection(_settings).Build();
            });

            builder.UseEnvironment("Testing");

            builder.ConfigureTestServices(
                services =>
                {
                    services.RemoveAll(typeof(DbConfiguration));
                    

                    services.Configure<DbConfiguration>(config =>
                    {
                        config.ConnectionString = _container.ConnectionString;
                    });
                });
        }

        public async Task InitializeAsync()
        {
            await _container.StartAsync();

            InitialisePostgreSqlDb();
        } 

        public new async Task DisposeAsync() => await _container.DisposeAsync();

        private void InitialisePostgreSqlDb()
        {
            using var conn = new NpgsqlConnection(_container.ConnectionString);
            var script = Migrations.InitialDbSchema;
            using var command = new NpgsqlCommand(script, conn);
            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
        }
    }


}
