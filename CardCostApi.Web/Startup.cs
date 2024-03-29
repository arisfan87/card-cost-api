﻿using System;
using System.Linq;
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
            services.AddDbContext<CardCostContext>(
                opt =>
                {
                    opt.UseInMemoryDatabase(databaseName: "card_cost");
                    opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
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
                });
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

                using var scope = app.ApplicationServices.CreateScope();
                var context = scope.ServiceProvider.GetService<CardCostContext>();

                if (!context.CardCosts.ToListAsync().Result.Any())
                {
                    context.Database.EnsureCreated();
                    AddTestData(context);
                }
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

        private static void AddTestData(CardCostContext context)
        {
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