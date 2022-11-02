using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GodelTech.Data;
using GodelTech.Data.EntityFrameworkCore.Simple;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business.Contracts;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business.Models;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Entities;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business
{
    public class BankService : IBankService
    {
        private readonly ISimpleRepository<BankEntity, Guid> _repository;
        private readonly IMapper _mapper;

        public BankService(ISimpleRepository<BankEntity, Guid> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IList<BankDto>> GetListAsync(CancellationToken cancellationToken)
        {
            return await _repository
                .GetListAsync<BankDto, BankEntity, Guid>(cancellationToken: cancellationToken);
        }

        public async Task<BankDto> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _repository
                .GetAsync<BankDto, BankEntity, Guid>(id, cancellationToken);
        }

        public Task<BankDto> AddAsync(IBankAddDto item, CancellationToken cancellationToken)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return AddInternalAsync(item, cancellationToken);
        }

        private async Task<BankDto> AddInternalAsync(IBankAddDto item, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<IBankAddDto, BankEntity>(item);

            entity = await _repository
                .InsertAsync(entity, cancellationToken);

            return _mapper.Map<BankEntity, BankDto>(entity);
        }

        public Task<BankDto> EditAsync(IBankEditDto item, CancellationToken cancellationToken)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return EditInternalAsync(item, cancellationToken);
        }

        private async Task<BankDto> EditInternalAsync(IBankEditDto item, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .GetAsync(item.Id, cancellationToken);

            if (entity == null)
            {
                return null;
            }

            _mapper.Map(item, entity);

            entity = await _repository.UpdateAsync(entity, cancellationToken: cancellationToken);

            return _mapper.Map<BankEntity, BankDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(id, cancellationToken);

            return true;
        }
    }
}
