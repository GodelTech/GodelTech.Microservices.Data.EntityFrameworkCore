using System;
using System.Collections.Generic;
using GodelTech.Data;
using GodelTech.Data.AutoMapper;
using GodelTech.Data.EntityFrameworkCore;
using GodelTech.Microservices.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly: CLSCompliant(false)]
namespace GodelTech.Microservices.Data.EntityFrameworkCore
{
    /// <summary>
    /// Data initializer.
    /// </summary>
#pragma warning disable S2436 // Reduce the number of generic parameters in the 'DataInitializer' class to no more than the 2 authorized.
    public class DataInitializer<TDbContext, TIUnitOfWork, TUnitOfWork> : MicroserviceInitializerBase
#pragma warning restore S2436 // Reduce the number of generic parameters in the 'DataInitializer' class to no more than the 2 authorized.
        where TDbContext : DbContext
        where TIUnitOfWork : IUnitOfWork
        where TUnitOfWork : UnitOfWork<TDbContext>, IUnitOfWork
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly DataInitializerOptions _options = new DataInitializerOptions();
        private readonly Action<SqlServerDbContextOptionsBuilder> _sqlServerOptionsAction;

        private readonly IList<Action<IServiceCollection>> _configureServicesList = new List<Action<IServiceCollection>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DataInitializer{TDbContext, TIUnitOfWork, TUnitOfWork}"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        /// <param name="hostEnvironment">Hosting environment.</param>
        /// <param name="configure">An <see cref="Action{DataInitializerOptions}"/> to configure the provided <see cref="DataInitializerOptions"/>.</param>
        /// <param name="sqlServerOptionsAction">An optional action to allow additional SQL Server specific configuration.</param>
        public DataInitializer(
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

            services
                .AddDbContextFactory<TDbContext>(
                    options => options
                        .UseSqlServer(
                            Configuration.GetConnectionString(_options.ConnectionStringName),
                            ConfigureSqlServerDbContextOptionsBuilder
                        )
                        .EnableSensitiveDataLogging(_hostEnvironment.IsDevelopment())
                );

            // Repositories
            foreach (var action in _configureServicesList)
            {
                action.Invoke(services);
            }

            // UnitOfWork
            services.AddScoped(typeof(TIUnitOfWork), typeof(TUnitOfWork));
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
        /// Adds registration of Repository
        /// </summary>
        /// <typeparam name="TIRepository">Interface of repository.</typeparam>
        /// <typeparam name="TRepository">Implementation of repository.</typeparam>
        /// <typeparam name="TEntity">The type of the T entity.</typeparam>
        /// <typeparam name="TKey">The type of the T key.</typeparam>
        /// <returns>DataInitializer.</returns>
#pragma warning disable S2436 // Reduce the number of generic parameters in the 'DataInitializer.WithRepository' method to no more than the 3 authorized.
        public DataInitializer<TDbContext, TIUnitOfWork, TUnitOfWork> WithRepository<TIRepository, TRepository, TEntity, TKey>()
#pragma warning restore S2436 // Reduce the number of generic parameters in the 'DataInitializer.WithRepository' method to no more than the 3 authorized.
            where TIRepository : IRepository<TEntity, TKey>
            where TRepository : Repository<TEntity, TKey>, TIRepository
            where TEntity : class, IEntity<TKey>
        {
            _configureServicesList.Add(
                services => services
                    .AddScoped<Func<TDbContext, TIRepository>>(
                        provider =>
                        {
                            return context =>
                            {
                                return ActivatorUtilities.CreateInstance<TRepository>(
                                    provider,
                                    context,
                                    provider.GetRequiredService<IDataMapper>()
                                );
                            };
                        }
                    )
            );

            return this;
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
        protected virtual void MigrateDatabase(IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            var dbContextFactory = app.ApplicationServices.GetRequiredService<IDbContextFactory<TDbContext>>();

            using var dbContext = dbContextFactory.CreateDbContext();

            dbContext.Database.Migrate();
        }
    }
}
