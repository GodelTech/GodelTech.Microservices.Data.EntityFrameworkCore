using System;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business.Contracts
{
    public interface IBankEditDto
    {
        Guid Id { get; }

        string Name { get; }
    }
}
