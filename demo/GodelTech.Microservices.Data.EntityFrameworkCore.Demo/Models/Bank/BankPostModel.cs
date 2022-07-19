using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business.Contracts;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Models.Bank
{
    public class BankPostModel : IBankAddDto
    {
        public string Name { get; set; }
    }
}
