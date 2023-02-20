using System.Linq;
using CardCostApi.Infrastructure.Store;
using CardCostApi.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CardCostApi.Test
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Startup>
    {
        /// <summary>
        /// We make sure each test class use a unique in memory db.
        /// <see href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.storage.inmemorydatabaseroot?view=efcore-5.0"></see>
        /// </summary>
        private readonly InMemoryDatabaseRoot _dbRoot = new InMemoryDatabaseRoot();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(
                config => { config.AddEnvironmentVariables("ASPNETCORE"); });

            builder.ConfigureTestServices(
                services =>
                {
                    //var dbContext =
                    //    services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<CardCostContext>));

                    //if (dbContext != null) services.Remove(dbContext);

                    //services.AddDbContext<CardCostContext>(
                    //    o =>
                    //    {
                    //        o.UseInMemoryDatabase("TestDb", _dbRoot);
                    //        o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    //    });
                });

            builder.UseEnvironment("Testing");
        }
    }
}