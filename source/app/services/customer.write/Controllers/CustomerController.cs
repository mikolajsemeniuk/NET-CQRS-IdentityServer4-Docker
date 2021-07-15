using System;
using System.Threading.Tasks;
using customer.write.Inputs;
using customer.write.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace customer.write.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly ICustomerRepository _repository;

        public CustomerController(ICustomerRepository repository) =>
            _repository = repository;
    
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddCustomerAsync([FromBody] CustomerInput input)
        {
            var result = await _repository.AddCustomerAsync(input);
            return result.Match<ActionResult>(
                created => Ok(created.Message));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomerAsync([FromRoute] Guid id, [FromBody] CustomerInput input)
        {
            var result = await _repository.UpdateCustomerAsync(id, input);
            return result.Match<ActionResult>(
                updated => Ok(updated.Message),
                invalidId => NotFound(invalidId.Message));
        }

        [Authorize(Policy = "Admin")]
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
