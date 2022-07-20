using System;
using System.Collections.Generic;
using GodelTech.Data;
using GodelTech.Data.EntityFrameworkCore;
using GodelTech.Microservices.Core;
using GodelTech.Microservices.Core.Mvc;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business.Contracts;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Contracts;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo
{
    public class Startup : MicroserviceStartup
    {
        private readonly IHostEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostEnvironment hostingEnvironment)
            : base(configuration)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        protected override IEnumerable<IMicroserviceInitializer> CreateInitializers()
        {
            yield return new DeveloperExceptionPageInitializer();
            yield return new HstsInitializer();

            yield return new GenericInitializer(null, (app, _) => app.UseRouting());

            yield return new DataInitializer<CurrencyExchangeRateDbContext, ICurrencyExchangeRateUnitOfWork, CurrencyExchangeRateUnitOfWork>(
                    Configuration,
                    _hostingEnvironment,
                    options => Configuration.Bind("DataInitializerOptions", options)
                )
                .WithRepository<IRepository<BankEntity, Guid>, Repository<BankEntity, Guid>, BankEntity, Guid>()
                .WithRepository<ICurrencyRepository, CurrencyRepository, CurrencyEntity, int>();

            yield return new GenericInitializer(services => services.AddTransient<IBankService, BankService>());

            yield return new ApiInitializer();

            yield return new GenericInitializer(
                null,
                (app, _) => app.UseEndpoints(
                    endpoints => endpoints.MapHealthChecks("/health")
                )
            );
        }
    }
}
