using System.Threading;
using System.Threading.Tasks;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Controllers
{
    [Route("currencies")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCountAsync(CancellationToken cancellationToken)
        {
            return Ok(
                await _currencyService.GetCountAsync(cancellationToken)
            );
        }
    }
}
