using AutoMapper;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business.Models;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Models.Bank;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Mappings
{
    public class BankProfile : Profile
    {
        public BankProfile()
        {
            CreateMap<BankDto, BankModel>();
        }
    }
}
