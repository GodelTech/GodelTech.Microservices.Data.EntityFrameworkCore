using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GodelTech.Data;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business.Contracts;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business.Models;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Contracts;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Entities;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business
{
    public class BankService : IBankService
    {
        private readonly ICurrencyExchangeRateUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BankService(ICurrencyExchangeRateUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IList<BankDto>> GetListAsync()
        {
            return await _unitOfWork.BankRepository
                .GetListAsync<BankDto, BankEntity, Guid>();
        }

        public async Task<BankDto> GetAsync(Guid id)
        {
            return await _unitOfWork.BankRepository
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

            entity = await _unitOfWork.BankRepository
                .InsertAsync(entity);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<BankEntity, BankDto>(entity);
        }

        public Task<BankDto> EditAsync(IBankEditDto item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return EditInternalAsync(item);
        }

        private async Task<BankDto> EditInternalAsync(IBankEditDto item)
        {
            var entity = await _unitOfWork.BankRepository
                .GetAsync(item.Id);

            if (entity == null)
            {
                return null;
            }

            _mapper.Map(item, entity);

            entity = _unitOfWork.BankRepository.Update(entity);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<BankEntity, BankDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            _unitOfWork.BankRepository.Delete(id);

            var result = await _unitOfWork.CommitAsync();

            return result == 1;
        }
    }
}
