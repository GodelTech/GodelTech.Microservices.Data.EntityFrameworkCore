using System;
using System.Collections.Generic;
using System.Linq;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Contracts;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Models.Bank;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Controllers
{
    [Route("banks")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly ICurrencyExchangeRateUnitOfWork _unitOfWork;

        private static readonly IReadOnlyList<BankModel> Items = new List<BankModel>
        {
            new BankModel(), new BankModel {Id = 1, Name = "Test Title"}
        };

        public BankController(ICurrencyExchangeRateUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<BankModel>), StatusCodes.Status200OK)]
        public IActionResult GetList()
        {
            return Ok(Items);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BankModel), StatusCodes.Status200OK)]
        public IActionResult Get(int id)
        {
            var item = Items.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BankModel), StatusCodes.Status201Created)]
        public IActionResult Post([FromBody] BankPostModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var item = new BankModel { Id = Items.Max(x => x.Id) + 1, Name = model.Name };

            return CreatedAtAction(
                nameof(Get),
                new { id = item.Id },
                item
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Put(int id, BankPutModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            if (id != model.Id)
            {
                return BadRequest();
            }

            var item = Items.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Delete(int id)
        {
            var item = Items.FirstOrDefault(x => x.Id == id);

            // delete functional here
            var result = item != null;

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
