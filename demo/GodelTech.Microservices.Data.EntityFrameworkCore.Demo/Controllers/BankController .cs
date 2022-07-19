using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Business.Contracts;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Models.Bank;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Controllers
{
    [Route("banks")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;
        private readonly IMapper _mapper;

        public BankController(IBankService bankService, IMapper mapper)
        {
            _bankService = bankService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<BankModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListAsync()
        {
            return Ok(
                _mapper.Map<IList<BankModel>>(
                    await _bankService.GetListAsync()
                )
            );
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetAsync))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BankModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var item = await _bankService.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(
                _mapper.Map<BankModel>(item)
            );
        }

        [HttpPost]
        [ProducesResponseType(typeof(BankModel), StatusCodes.Status201Created)]
        public async Task<IActionResult> PostAsync([FromBody] BankPostModel model)
        {
            var item = _mapper.Map<BankModel>(
                await _bankService.AddAsync(model)
            );

            return CreatedAtAction(
                nameof(GetAsync),
                new { id = item.Id },
                item
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<IActionResult> PutAsync(Guid id, BankPutModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            return PutInternalAsync(id, model);
        }

        private async Task<IActionResult> PutInternalAsync(Guid id, BankPutModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var item = await _bankService.EditAsync(model);

            if (item == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await _bankService.DeleteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
