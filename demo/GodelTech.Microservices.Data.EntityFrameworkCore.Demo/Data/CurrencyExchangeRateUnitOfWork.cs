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
            Func<CurrencyExchangeRateDbContext, IRepository<BankEntity, int>> bankRepository,
            IDbContextFactory<CurrencyExchangeRateDbContext> dbContextFactory)
            : base(dbContextFactory)
        {
            if (bankRepository == null) throw new ArgumentNullException(nameof(bankRepository));

            RegisterRepository(bankRepository(DbContext));
        }

        public IRepository<BankEntity, int> BankRepository => GetRepository<BankEntity, int>();
    }
}
