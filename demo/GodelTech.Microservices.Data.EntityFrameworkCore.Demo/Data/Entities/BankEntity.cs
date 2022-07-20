using System;
using GodelTech.Data.EntityFrameworkCore;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Entities
{
    public class BankEntity : Entity<Guid>
    {
        public string Name { get; set; }
    }
}
