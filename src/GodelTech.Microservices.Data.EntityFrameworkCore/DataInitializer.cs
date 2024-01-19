using System;
using GodelTech.Data;
using GodelTech.Data.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GodelTech.Microservices.Data.EntityFrameworkCore
{
    /// <summary>
    /// Data initializer.
    /// </summary>
#pragma warning disable S2436 // Reduce the number of generic parameters in the 'DataInitializer' class to no more than the 2 authorized.
    public class DataInitializer<TDbContext, TIUnitOfWork, TUnitOfWork> : DataInitializerBase<TDbContext>
#pragma warning restore S2436 // Reduce the number of generic parameters in the 'DataInitializer' class to no more than the 2 authorized.
        where TDbContext : DbContext
        where TIUnitOfWork : IUnitOfWork
        where TUnitOfWork : UnitOfWork<TDbContext>, IUnitOfWork
    {
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
            : base(
                configuration,
                hostEnvironment,
                configure,
                sqlServerOptionsAction)
        {

        }

        /// <inheritdoc />
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services
                .AddDbContextFactory<TDbContext>(
                    ConfigureDbContextOptionsBuilder
                );

            // UnitOfWork
            services.AddScoped(typeof(TIUnitOfWork), typeof(TUnitOfWork));
        }

        /// <inheritdoc />
        protected override void MigrateDatabase(IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);

            var dbContextFactory = app.ApplicationServices.GetRequiredService<IDbContextFactory<TDbContext>>();
            using var dbContext = dbContextFactory.CreateDbContext();

            dbContext.Database.Migrate();
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
            ConfigureServicesList.Add(
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
    }
}
