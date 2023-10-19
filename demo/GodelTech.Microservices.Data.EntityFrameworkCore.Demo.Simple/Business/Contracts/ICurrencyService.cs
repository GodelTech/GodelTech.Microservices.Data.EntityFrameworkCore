using System.Threading;
using System.Threading.Tasks;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Business.Contracts
{
    public interface ICurrencyService
    {
        Task<int> GetCountAsync(CancellationToken cancellationToken);
    }
}
