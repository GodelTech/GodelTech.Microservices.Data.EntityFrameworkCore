# GodelTech.Microservices.Data.EntityFrameworkCore
Microservice initializer for [GodelTech.Data.EntityFrameworkCore](https://github.com/GodelTech/GodelTech.Data.EntityFrameworkCore)

For Repository with Unit of Work use:
```c#
yield return new DataInitializer<CurrencyExchangeRateDbContext, ICurrencyExchangeRateUnitOfWork, CurrencyExchangeRateUnitOfWork>(
        Configuration,
        _hostingEnvironment,
        options => Configuration.Bind("DataInitializerOptions", options)
    )
    .WithRepository<IRepository<BankEntity, Guid>, Repository<BankEntity, Guid>, BankEntity, Guid>()
    .WithRepository<ICurrencyRepository, CurrencyRepository, CurrencyEntity, int>();
```

For simple Repository (repository without Unit of Work) use:
```c#
yield return new SimpleDataInitializer<CurrencyExchangeRateDbContext>(
        Configuration,
        _hostingEnvironment,
        options => Configuration.Bind("DataInitializerOptions", options)
    )
    .WithRepository<ICurrencyRepository, CurrencyRepository, CurrencyEntity, int>();
```