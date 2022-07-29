using System;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business.Contracts
{
    public interface IBankEditDto
    {
        Guid Id { get; }

        string Name { get; }
    }
}
