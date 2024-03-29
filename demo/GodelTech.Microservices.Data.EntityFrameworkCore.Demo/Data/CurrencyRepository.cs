﻿using GodelTech.Data;
using GodelTech.Data.EntityFrameworkCore;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Contracts;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Entities;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data
{
    public class CurrencyRepository : Repository<CurrencyEntity, int>, ICurrencyRepository
    {
        public CurrencyRepository(CurrencyExchangeRateDbContext dbContext, IDataMapper dataMapper)
            : base(dbContext, dataMapper)
        {

        }
    }
}
