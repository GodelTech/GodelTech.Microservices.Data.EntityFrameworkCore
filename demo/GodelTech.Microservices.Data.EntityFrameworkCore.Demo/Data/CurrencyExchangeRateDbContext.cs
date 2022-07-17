using System;
using GodelTech.Data.EntityFrameworkCore;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data
{
    public class CurrencyExchangeRateDbContext : DbContextBase
    {
        public CurrencyExchangeRateDbContext(DbContextOptions<CurrencyExchangeRateDbContext> dbContextOptions)
            : base(dbContextOptions, "currencyExchangeRate")
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BankConfiguration(SchemaName));
            modelBuilder.ApplyConfiguration(new CurrencyConfiguration(SchemaName));
        }
    }
}
