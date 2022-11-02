using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GodelTech.Data;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business.Contracts;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business.Models;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Contracts;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Entities;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business
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

        public async Task<IList<BankDto>> GetListAsync(CancellationToken cancellationToken)
        {
            return await _unitOfWork.BankRepository
                .GetListAsync<BankDto, BankEntity, Guid>(cancellationToken: cancellationToken);
        }

        public async Task<BankDto> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _unitOfWork.BankRepository
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

            entity = await _unitOfWork.BankRepository
                .InsertAsync(entity, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return _mapper.Map<BankEntity, BankDto>(entity);
        }

        public Task<BankDto> EditAsync(IBankEditDto item, CancellationToken cancellationToken)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return EditInternalAsync(item, cancellationToken);
        }

        private async Task<BankDto> EditInternalAsync(IBankEditDto item, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.BankRepository
                .GetAsync(item.Id, cancellationToken);

            if (entity == null)
            {
                return null;
            }

            _mapper.Map(item, entity);

            entity = _unitOfWork.BankRepository.Update(entity);

            await _unitOfWork.CommitAsync(cancellationToken);

            return _mapper.Map<BankEntity, BankDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            _unitOfWork.BankRepository.Delete(id);

            var result = await _unitOfWork.CommitAsync(cancellationToken);

            return result == 1;
        }
    }
}
