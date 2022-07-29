using System;
using GodelTech.Data.EntityFrameworkCore.Simple;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Entities;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Contracts
{
    public interface ICurrencyRepository : ISimpleRepository<CurrencyEntity, int>
    {
        [Obsolete("Insert operation is not allowed.", true)]
        new CurrencyEntity Insert(CurrencyEntity entity);
    }
}
