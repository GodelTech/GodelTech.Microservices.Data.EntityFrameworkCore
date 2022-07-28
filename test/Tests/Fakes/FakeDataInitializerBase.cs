using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeDataInitializerBase : DataInitializerBase<DbContext>
    {
        public FakeDataInitializerBase(
            IConfiguration configuration,
            IHostEnvironment hostEnvironment,
            Action<DataInitializerOptions> configure = null,
            Action<SqlServerDbContextOptionsBuilder> sqlServerOptionsAction = null)
            : base(configuration, hostEnvironment, configure, sqlServerOptionsAction)
        {

        }

        public IList<Action<IServiceCollection>> ExposedConfigureServicesList => ConfigureServicesList;

        public void ExposedConfigureDbContextOptionsBuilder(DbContextOptionsBuilder options)
        {
            base.ConfigureDbContextOptionsBuilder(options);
        }

        public void ExposedConfigureSqlServerDbContextOptionsBuilder(SqlServerDbContextOptionsBuilder options)
        {
            base.ConfigureSqlServerDbContextOptionsBuilder(options);
        }

        protected override void MigrateDatabase(IApplicationBuilder app)
        {
            throw new NotImplementedException();
        }
    }
}
