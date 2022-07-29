using AutoMapper;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business.Models;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Models.Bank;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Mappings
{
    public class BankProfile : Profile
    {
        public BankProfile()
        {
            CreateMap<BankDto, BankModel>();
        }
    }
}
