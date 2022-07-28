using System.Collections.Generic;
using GodelTech.Microservices.Core;
using GodelTech.Microservices.Core.Mvc;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business.Contracts;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Contracts;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple
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

            yield return new SimpleDataInitializer<CurrencyExchangeRateDbContext>(
                    Configuration,
                    _hostingEnvironment,
                    options => Configuration.Bind("DataInitializerOptions", options)
                )
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
