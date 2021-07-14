using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using customer.write.Inputs;
using customer.write.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace customer.write.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _repository;

        public CustomerController(ICustomerRepository repository) =>
            _repository = repository;

        [HttpGet]
        public async Task<ActionResult> Get() => 
            Ok(await _repository.GetCustomersAsync());

        [HttpGet("auth")]
        [Authorize]
        public string Auth() => "auth me";

        [HttpPost]
        public async Task<ActionResult> AddCustomerAsync([FromBody] CustomerInput input)
        {
            var result = await _repository.AddCustomerAsync(input);
            return result.Match<ActionResult>(
                created => Ok(created.Message));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomerAsync([FromRoute] Guid id, [FromBody] CustomerInput input)
        {
            var result = await _repository.UpdateCustomerAsync(id, input);
            return result.Match<ActionResult>(
                updated => Ok(updated.Message),
                invalidId => NotFound(invalidId.Message));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomerAsync(Guid id)
        {
            var result = await _repository.RemoveCustomerAsync(id);
            return result.Match<ActionResult>(
                removed => Ok(removed.Message),
                invalidId => NotFound(invalidId.Message));
        }
    }
}
