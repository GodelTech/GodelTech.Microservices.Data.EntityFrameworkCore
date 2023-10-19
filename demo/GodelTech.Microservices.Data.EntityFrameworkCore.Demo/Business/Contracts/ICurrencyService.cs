using System.Threading.Tasks;
using System.Threading;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business.Contracts
{
    public interface ICurrencyService
    {
        Task<int> GetCountAsync(CancellationToken cancellationToken);
    }
}
