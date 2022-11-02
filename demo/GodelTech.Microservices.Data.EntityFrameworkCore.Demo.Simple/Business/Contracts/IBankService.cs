using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business.Models;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business.Contracts
{
    public interface IBankService
    {
        Task<IList<BankDto>> GetListAsync(CancellationToken cancellationToken);

        Task<BankDto> GetAsync(Guid id, CancellationToken cancellationToken);

        Task<BankDto> AddAsync(IBankAddDto item, CancellationToken cancellationToken);

        Task<BankDto> EditAsync(IBankEditDto item, CancellationToken cancellationToken);

        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
