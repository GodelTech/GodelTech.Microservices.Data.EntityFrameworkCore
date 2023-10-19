using System.Threading;
using System.Threading.Tasks;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business.Contracts;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Contracts;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyExchangeRateUnitOfWork _unitOfWork;

        public CurrencyService(ICurrencyExchangeRateUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetCountAsync(CancellationToken cancellationToken)
        {
            return await _unitOfWork.CurrencyRepository.CountAsync(null, cancellationToken);
        }
    }
}
