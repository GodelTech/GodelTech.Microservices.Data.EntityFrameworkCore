using System;
using GodelTech.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.IntegrationTests.Fakes
{
    public class FakeDataInitializer : DataInitializer<DbContext, IUnitOfWork, FakeUnitOfWork>
    {
        public FakeDataInitializer(
            IConfiguration configuration, IHostEnvironment hostEnvironment,
            Action<DataInitializerOptions> configure = null,
            Action<SqlServerDbContextOptionsBuilder> sqlServerOptionsAction = null)
            : base(
                configuration,
                hostEnvironment,
                configure,
                sqlServerOptionsAction)
        {

        }

        public void ExposedMigrateDatabase(IApplicationBuilder app)
        {
            base.MigrateDatabase(app);
        }
    }
}
