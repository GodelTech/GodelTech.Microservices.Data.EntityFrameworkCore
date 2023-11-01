using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
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
        public ITestOutputHelper Output { get; set; }

        public CurrencyExchangeRateDbContext GetDbContext()
        {
            using var scope = Services.CreateScope();

            var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<CurrencyExchangeRateDbContext>>();

            var dbContext = dbContextFactory.CreateDbContext();

            dbContext.Database.EnsureCreated();

            return dbContext;
        }

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
                                        "DataSource=:memory:"
                                    ),
                                    new KeyValuePair<string, string>(
                                        "DataInitializerOptions:EnableDatabaseMigration",
                                        true.ToString()
                                    )
                                }
                            );
                    }
                )
                .ConfigureServices(
                    services =>
                    {
                        var factoryDescriptor = services.Single(x => x.ServiceType == typeof(IDbContextFactory<CurrencyExchangeRateDbContext>));
#pragma warning disable EF1001 // Internal EF Core API usage.
                        var factorySourceDescriptor = services.Single(x => x.ServiceType == typeof(IDbContextFactorySource<CurrencyExchangeRateDbContext>));
#pragma warning restore EF1001 // Internal EF Core API usage.
                        var genericOptionsDescriptor = services.Single(x => x.ServiceType == typeof(DbContextOptions));
                        var optionsDescriptor = services.Single(x => x.ServiceType == typeof(DbContextOptions<CurrencyExchangeRateDbContext>));

                        // remove IDbContextFactory to allow override
                        services.Remove(factoryDescriptor);
                        services.Remove(factorySourceDescriptor);
                        services.Remove(genericOptionsDescriptor);
                        services.Remove(optionsDescriptor);

                        // Create open SqliteConnection so EF won't automatically close it.
                        services.AddSingleton<DbConnection>(
                            _ =>
                            {
                                var connection = new SqliteConnection("DataSource=:memory:");

                                connection.CreateFunction("newsequentialid", Guid.NewGuid);

                                connection.Open();

                                return connection;
                            }
                        );

                        services.AddDbContextFactory<CurrencyExchangeRateDbContext>(
                            (provider, optionsBuilder) =>
                            {
                                var connection = provider.GetRequiredService<DbConnection>();

                                optionsBuilder
                                    .UseSqlite(connection)
                                    .EnableSensitiveDataLogging();
                            }
                        );
                    }
                );
        }
    }
}
