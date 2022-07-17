using System;
using GodelTech.Data;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Entities;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Contracts
{
    public interface ICurrencyRepository : IRepository<CurrencyEntity, int>
    {
        [Obsolete("Insert operation is not allowed.", true)]
        new CurrencyEntity Insert(CurrencyEntity entity);
    }
}
