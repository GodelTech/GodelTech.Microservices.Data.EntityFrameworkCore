using System;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business.Contracts;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Models.Bank
{
    public class BankPutModel : IBankEditDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
