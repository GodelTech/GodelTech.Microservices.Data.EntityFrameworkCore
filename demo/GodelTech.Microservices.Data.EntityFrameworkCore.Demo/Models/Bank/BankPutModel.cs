using System;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business.Contracts;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Models.Bank
{
    public class BankPutModel : IBankEditDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
