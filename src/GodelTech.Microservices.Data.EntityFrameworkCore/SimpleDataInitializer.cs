using System;
using GodelTech.Data;
using GodelTech.Data.EntityFrameworkCore.Simple;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GodelTech.Microservices.Data.EntityFrameworkCore
{
    /// <summary>
    /// Simple Data initializer. Repository automatically saves changes.
    /// </summary>
    public class SimpleDataInitializer<TDbContext> : DataInitializerBase<TDbContext>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDataInitializer{TDbContext}"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        /// <param name="hostEnvironment">Hosting environment.</param>
        /// <param name="configure">An <see cref="Action{DataInitializerOptions}"/> to configure the provided <see cref="DataInitializerOptions"/>.</param>
        /// <param name="sqlServerOptionsAction">An optional action to allow additional SQL Server specific configuration.</param>
        public SimpleDataInitializer(
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

            services.AddDbContext<TDbContext>(ConfigureDbContextOptionsBuilder);

            // Repositories
            services.AddScoped<DbContext>(x => x.GetRequiredService<TDbContext>());
            services.AddScoped(typeof(ISimpleRepository<,>), typeof(SimpleRepository<,>));
        }

        /// <inheritdoc />
        protected override void MigrateDatabase(IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var dbContext = serviceScope.ServiceProvider.GetRequiredService<TDbContext>();

            dbContext.Database.Migrate();
        }

        /// <summary>
        /// Adds registration of Repository
        /// </summary>
        /// <typeparam name="TIRepository">Interface of repository.</typeparam>
        /// <typeparam name="TRepository">Implementation of repository.</typeparam>
        /// <typeparam name="TEntity">The type of the T entity.</typeparam>
        /// <typeparam name="TKey">The type of the T key.</typeparam>
        /// <returns>SimpleDataInitializer.</returns>
#pragma warning disable S2436 // Reduce the number of generic parameters in the 'SimpleDataInitializer.WithRepository' method to no more than the 3 authorized.
        public SimpleDataInitializer<TDbContext> WithRepository<TIRepository, TRepository, TEntity, TKey>()
#pragma warning restore S2436 // Reduce the number of generic parameters in the 'SimpleDataInitializer.WithRepository' method to no more than the 3 authorized.
            where TIRepository : class, ISimpleRepository<TEntity, TKey>
            where TRepository : SimpleRepository<TEntity, TKey>, TIRepository
            where TEntity : class, IEntity<TKey>
        {
            ConfigureServicesList.Add(
                services => services
                    .AddScoped<TIRepository, TRepository>()
            );

            return this;
        }
    }
}
