using AutoMapper;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business.Contracts;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business.Models;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Entities;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business.Mappings
{
    public class BankProfile : Profile
    {
        public BankProfile()
        {
            CreateMap<BankEntity, BankDto>();

            CreateMap<IBankAddDto, BankEntity>();

            CreateMap<IBankEditDto, BankEntity>();
        }
    }
}
