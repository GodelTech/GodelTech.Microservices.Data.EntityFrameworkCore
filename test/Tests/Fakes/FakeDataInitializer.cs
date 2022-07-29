using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeSimpleDataInitializer : SimpleDataInitializer<DbContext>
    {
        public FakeSimpleDataInitializer(
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
