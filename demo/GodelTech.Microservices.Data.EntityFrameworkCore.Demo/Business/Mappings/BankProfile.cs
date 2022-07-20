using AutoMapper;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business.Contracts;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business.Models;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Entities;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business.Mappings
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
