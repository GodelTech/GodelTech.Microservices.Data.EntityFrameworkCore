using System;
using GodelTech.Data;
using GodelTech.Data.EntityFrameworkCore;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Contracts;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data
{
    public class CurrencyExchangeRateUnitOfWork : UnitOfWork<CurrencyExchangeRateDbContext>, ICurrencyExchangeRateUnitOfWork
    {
        public CurrencyExchangeRateUnitOfWork(
            Func<CurrencyExchangeRateDbContext, IRepository<BankEntity, Guid>> bankRepository,
            Func<CurrencyExchangeRateDbContext, ICurrencyRepository> currencyRepository,
            IDbContextFactory<CurrencyExchangeRateDbContext> dbContextFactory)
            : base(dbContextFactory)
        {
            ArgumentNullException.ThrowIfNull(bankRepository);
            ArgumentNullException.ThrowIfNull(currencyRepository);

            RegisterRepository(bankRepository(DbContext));
            RegisterRepository(currencyRepository(DbContext));
        }

        public IRepository<BankEntity, Guid> BankRepository => GetRepository<BankEntity, Guid>();

        public ICurrencyRepository CurrencyRepository => GetRepository<ICurrencyRepository, CurrencyEntity, int>();
    }
}
