using System;
using System.Collections.Generic;
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

        public async Task<IList<BankDto>> GetListAsync()
        {
            return await _repository
                .GetListAsync<BankDto, BankEntity, Guid>();
        }

        public async Task<BankDto> GetAsync(Guid id)
        {
            return await _repository
                .GetAsync<BankDto, BankEntity, Guid>(id);
        }

        public Task<BankDto> AddAsync(IBankAddDto item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return AddInternalAsync(item);
        }

        private async Task<BankDto> AddInternalAsync(IBankAddDto item)
        {
            var entity = _mapper.Map<IBankAddDto, BankEntity>(item);

            entity = await _repository
                .InsertAsync(entity);

            return _mapper.Map<BankEntity, BankDto>(entity);
        }

        public Task<BankDto> EditAsync(IBankEditDto item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return EditInternalAsync(item);
        }

        private async Task<BankDto> EditInternalAsync(IBankEditDto item)
        {
            var entity = await _repository
                .GetAsync(item.Id);

            if (entity == null)
            {
                return null;
            }

            _mapper.Map(item, entity);

            entity = await _repository.UpdateAsync(entity);

            return _mapper.Map<BankEntity, BankDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);

            await _repository.DeleteAsync(entity);

            return true;
        }
    }
}
