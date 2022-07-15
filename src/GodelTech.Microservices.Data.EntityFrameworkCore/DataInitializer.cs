using System;
using GodelTech.Microservices.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: CLSCompliant(false)]
namespace GodelTech.Microservices.Data.EntityFrameworkCore
{
    /// <summary>
    /// Data initializer.
    /// </summary>
    public class DataInitializer : IMicroserviceInitializer
    {
        /// <inheritdoc />
        public void ConfigureServices(IServiceCollection services)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            throw new NotImplementedException();
        }
    }
}
