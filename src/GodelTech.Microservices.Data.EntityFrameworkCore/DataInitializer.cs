using System;
using GodelTech.Microservices.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: CLSCompliant(false)]
namespace GodelTech.Microservices.Data.EntityFrameworkCore
{
    public class DataInitializer : IMicroserviceInitializer
    {
        public void ConfigureServices(IServiceCollection services)
        {
            throw new NotImplementedException();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            throw new NotImplementedException();
        }
    }
}
