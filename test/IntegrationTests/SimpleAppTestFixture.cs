using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
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
        public ITestOutputHelper Output { get; set; }

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
                        var genericOptionsDescriptor = services.Single(x => x.ServiceType == typeof(DbContextOptions));
                        var optionsDescriptor = services.Single(x => x.ServiceType == typeof(DbContextOptions<CurrencyExchangeRateDbContext>));

                        // remove IDbContext to allow override
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

                        services.AddDbContext<CurrencyExchangeRateDbContext>(
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
