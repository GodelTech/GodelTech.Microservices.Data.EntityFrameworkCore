using System;
using System.Collections.Generic;
using System.Linq;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.IntegrationTests
{
    public class AppTestFixture : WebApplicationFactory<Startup>
    {
        private readonly Guid _guid;

        public AppTestFixture()
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

            builder
                .ConfigureServices(
                    services =>
                    {
                        var factoryDescriptor = services.First(x =>
                            x.ServiceType == typeof(IDbContextFactory<CurrencyExchangeRateDbContext>));
#pragma warning disable EF1001 // Internal EF Core API usage.
                        var factorySourceDescriptor = services.First(x => x.ServiceType == typeof(IDbContextFactorySource<CurrencyExchangeRateDbContext>));
#pragma warning restore EF1001 // Internal EF Core API usage.
                        var genericOptionsDescriptor = services.First(x => x.ServiceType == typeof(DbContextOptions));
                        var optionsDescriptor = services.First(x =>
                            x.ServiceType == typeof(DbContextOptions<CurrencyExchangeRateDbContext>));

                        // remove IDbContextFactory to allow override
                        services.Remove(factoryDescriptor);
                        services.Remove(factorySourceDescriptor);
                        services.Remove(genericOptionsDescriptor);
                        services.Remove(optionsDescriptor);

                        services.AddDbContextFactory<CurrencyExchangeRateDbContext>(
                            ConfigureDbContextOptionsBuilder
                        );
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
                                        "DataInitializerOptions:EnableDatabaseMigration",
                                        false.ToString()
                                    )
                                }
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
