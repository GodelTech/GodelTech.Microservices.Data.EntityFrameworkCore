using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business.Contracts;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Models.Bank
{
    public class BankPostModel : IBankAddDto
    {
        public string Name { get; set; }
    }
}
