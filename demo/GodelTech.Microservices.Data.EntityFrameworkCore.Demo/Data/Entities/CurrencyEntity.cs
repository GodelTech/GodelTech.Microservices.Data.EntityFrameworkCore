using GodelTech.Data;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Entities
{
    public class CurrencyEntity : Entity<int>
    {
        public string AlphabeticCode { get; set; }
    }
}
