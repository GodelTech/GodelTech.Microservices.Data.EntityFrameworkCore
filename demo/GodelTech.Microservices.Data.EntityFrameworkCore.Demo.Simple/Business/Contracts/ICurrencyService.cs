using System.Threading.Tasks;
using System.Threading;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business.Contracts
{
    public interface ICurrencyService
    {
        Task<int> GetCountAsync(CancellationToken cancellationToken);
    }
}
