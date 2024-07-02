using GodelTech.Data;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Entities
{
    public class CurrencyEntity : Entity<int>
    {
        public string AlphabeticCode { get; set; }
    }
}
