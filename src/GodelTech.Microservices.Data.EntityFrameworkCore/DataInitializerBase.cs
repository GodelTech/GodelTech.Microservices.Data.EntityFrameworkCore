using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GodelTech.Data;
using GodelTech.Data.AutoMapper;
using GodelTech.Microservices.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly: CLSCompliant(false)]
[assembly: InternalsVisibleTo("GodelTech.Microservices.Data.EntityFrameworkCore.Tests")]
namespace GodelTech.Microservices.Data.EntityFrameworkCore
{
    /// <summary>
    /// Base Data initializer.
    /// </summary>
    public abstract class DataInitializerBase<TDbContext> : MicroserviceInitializerBase
        where TDbContext : DbContext
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly DataInitializerOptions _options = new DataInitializerOptions();
        private readonly Action<SqlServerDbContextOptionsBuilder> _sqlServerOptionsAction;

        private protected DataInitializerBase(
            IConfiguration configuration,
            IHostEnvironment hostEnvironment,
            Action<DataInitializerOptions> configure = null,
            Action<SqlServerDbContextOptionsBuilder> sqlServerOptionsAction = null)
            : base(configuration)
        {
            _hostEnvironment = hostEnvironment;
            configure?.Invoke(_options);
            _sqlServerOptionsAction = sqlServerOptionsAction;
        }

        /// <summary>
        /// List actions of services to configure.
        /// </summary>
        protected IList<Action<IServiceCollection>> ConfigureServicesList { get; } = new List<Action<IServiceCollection>>();

        /// <inheritdoc />
        public override void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHealthChecks()
                .AddSqlServer(Configuration.GetConnectionString(_options.ConnectionStringName), name: _options.SqlServerHealthCheckName);

            // AutoMapper
            services.AddAutoMapper(typeof(TDbContext).Assembly);

            // GodelTech.Data.AutoMapper
            services.AddScoped<IDataMapper, DataMapper>();

            // Services
            foreach (var action in ConfigureServicesList)
            {
                action.Invoke(services);
            }
        }

        /// <inheritdoc />
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (_options.EnableDatabaseMigration)
            {
                MigrateDatabase(app);
            }
        }

        /// <summary>
        /// Configure DbContextOptionsBuilder.
        /// </summary>
        /// <param name="options">DbContextOptionsBuilder.</param>
        protected virtual void ConfigureDbContextOptionsBuilder(DbContextOptionsBuilder options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            options
                .UseSqlServer(
                    Configuration.GetConnectionString(_options.ConnectionStringName),
                    ConfigureSqlServerDbContextOptionsBuilder
                )
                .EnableSensitiveDataLogging(_hostEnvironment.IsDevelopment());
        }

        /// <summary>
        /// Configure SqlServerDbContextOptionsBuilder.
        /// </summary>
        /// <param name="options">SqlServerDbContextOptionsBuilder.</param>
        protected virtual void ConfigureSqlServerDbContextOptionsBuilder(SqlServerDbContextOptionsBuilder options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            _sqlServerOptionsAction?.Invoke(options);

            options.EnableRetryOnFailure();
        }

        /// <summary>
        /// Migrate database.
        /// </summary>
        /// <param name="app">Application builder.</param>
        protected abstract void MigrateDatabase(IApplicationBuilder app);
    }
}
