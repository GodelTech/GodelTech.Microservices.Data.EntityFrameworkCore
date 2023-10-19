using System;
using System.Collections.Generic;
using System.Linq;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.IntegrationTests
{
    public class SimpleAppTestFixture : WebApplicationFactory<Startup>
    {
        private readonly Guid _guid;

        public SimpleAppTestFixture()
        {
            _guid = Guid.NewGuid();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<CurrencyExchangeRateDbContext>();

            ConfigureDbContextOptionsBuilder(dbContextOptionsBuilder);

            DbContext = new CurrencyExchangeRateDbContext(dbContextOptionsBuilder.Options);
        }

        public ITestOutputHelper Output { get; set; }
        public CurrencyExchangeRateDbContext DbContext { get; }

        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = base.CreateHostBuilder();

            builder.ConfigureLogging(
                logging =>
                {
                    logging.ClearProviders(); // Remove other loggers
                    logging.AddXUnit(Output); // Use the ITestOutputHelper instance
                }
            );

            return builder;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder
                .UseSetting("https_port", "8080")
                .ConfigureAppConfiguration(
                    configurationBuilder =>
                    {
                        configurationBuilder
                            .AddInMemoryCollection(
                                new KeyValuePair<string, string>[]
                                {
                                    new KeyValuePair<string, string>(
                                        "ConnectionStrings:DefaultConnection",
                                        $"Data Source=InMemoryDbForTesting{_guid:N};Mode=Memory;Cache=Shared"
                                    ),
                                    new KeyValuePair<string, string>(
                                        "DataInitializerOptions:EnableDatabaseMigration",
                                        false.ToString()
                                    )
                                }
                            );
                    }
                )
                .ConfigureServices(
                    services =>
                    {
                        var genericOptionsDescriptor = services.Single(x => x.ServiceType == typeof(DbContextOptions));
                        var optionsDescriptor = services.Single(x => x.ServiceType == typeof(DbContextOptions<CurrencyExchangeRateDbContext>));

                        // remove IDbContext to allow override
                        services.Remove(genericOptionsDescriptor);
                        services.Remove(optionsDescriptor);

                        services.AddDbContext<CurrencyExchangeRateDbContext>(
                            ConfigureDbContextOptionsBuilder
                        );
                    }
                );
        }

        private void ConfigureDbContextOptionsBuilder(DbContextOptionsBuilder builder)
        {
            builder
                .UseInMemoryDatabase($"InMemoryDbForTesting{_guid:N}")
                .EnableSensitiveDataLogging();
        }
    }
}
