using GodelTech.Data.EntityFrameworkCore;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Entities
{
    public class BankEntity : Entity<int>
    {
        public string Name { get; set; }
    }
}
