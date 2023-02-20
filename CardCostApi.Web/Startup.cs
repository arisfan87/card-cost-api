using System;
using System.Linq;
using System.Net.Http;
using CardCostApi.Core;
using CardCostApi.Core.Abstractions;
using CardCostApi.Core.Settings;
using CardCostApi.Infrastructure.BinList;
using CardCostApi.Infrastructure.Entities;
using CardCostApi.Infrastructure.Store;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using Serilog;

namespace CardCostApi.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DbConfiguration>(Configuration.GetRequiredSection("DbConfiguration"));
            services.AddOptions<DefaultCardCostSettings>()
                .Bind(Configuration.GetSection("DefaultCardCostSettings"))
                .ValidateDataAnnotations();

            services.AddSwaggerGen();
            services.AddControllers();
            services.AddHttpClient(
                "BinListClient",
                c =>
                {
                    c.BaseAddress = new Uri(Configuration.GetValue<string>("BinListBaseUrl"));
                    c.DefaultRequestHeaders.Add("Accept", "application/json");
                }).AddPolicyHandler(GetRetryPolicy());

            services.AddTransient<ΙBinListService, BinListService>();
            services.AddTransient<ICardCostService, CardCostService>();
            services.AddTransient<ICardCostConfigurationService, CardCostConfigurationService>();
            services.AddTransient<ICardCostConfigurationRepository, CardCostConfigurationRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI(
                c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Card Cost API V1");
                    c.RoutePrefix = "docs";
                });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(1));
        }
    }
}