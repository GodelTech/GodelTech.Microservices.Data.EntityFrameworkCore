using System.Threading;
using System.Threading.Tasks;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business.Contracts;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Contracts;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _repository;

        public CurrencyService(ICurrencyRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> GetCountAsync(CancellationToken cancellationToken)
        {
            return await _repository.CountAsync(null, cancellationToken);
        }
    }
}
