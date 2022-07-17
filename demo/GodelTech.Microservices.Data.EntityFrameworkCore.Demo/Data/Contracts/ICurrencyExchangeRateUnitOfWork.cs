using GodelTech.Data;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Entities;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Contracts
{
    public interface ICurrencyExchangeRateUnitOfWork : IUnitOfWork
    {
        IRepository<BankEntity, int> BankRepository { get; }
    }
}
