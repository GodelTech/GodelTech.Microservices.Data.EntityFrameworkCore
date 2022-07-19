using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business.Models;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business.Contracts
{
    public interface IBankService
    {
        Task<IList<BankDto>> GetListAsync();

        Task<BankDto> GetAsync(Guid id);

        Task<BankDto> AddAsync(IBankAddDto item);

        Task<BankDto> EditAsync(IBankEditDto item);

        Task<bool> DeleteAsync(Guid id);
    }
}
