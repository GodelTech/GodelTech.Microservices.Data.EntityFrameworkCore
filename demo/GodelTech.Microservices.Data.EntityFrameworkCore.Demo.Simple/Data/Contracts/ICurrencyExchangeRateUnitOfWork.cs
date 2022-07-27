using System;
using GodelTech.Data;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Entities;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Contracts
{
    public interface ICurrencyExchangeRateUnitOfWork : IUnitOfWork
    {
        IRepository<BankEntity, Guid> BankRepository { get; }

        ICurrencyRepository CurrencyRepository { get; }
    }
}
