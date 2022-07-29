using GodelTech.Data.EntityFrameworkCore;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Entities
{
    public class CurrencyEntity : EntityNoneDatabaseGeneratedIdentifier<int>
    {
        public string AlphabeticCode { get; set; }
    }
}
