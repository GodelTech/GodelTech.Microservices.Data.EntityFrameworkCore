# GodelTech.Microservices.Data.EntityFrameworkCore

## Description
GodelTech.Microservices.Data.EntityFrameworkCore is a .NET library that serves as a microservice initializer for [GodelTech.Data.EntityFrameworkCore](https://github.com/GodelTech/GodelTech.Data.EntityFrameworkCore). It provides features for setting up repositories with or without the Unit of Work pattern.

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

## License
This project is licensed under the MIT License. See the LICENSE file for more details.