using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using customer.read.Interfaces;
using customer.read.Payloads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace customer.read.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly ICustomerRepository _repository;
        public CustomerController(ICustomerRepository repository)
        {
            _repository = repository;
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerPayload>>> GetCustomersAsync() =>
            Ok(await _repository.GetCustomersAsync());

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerPayload>> GetCustomerAsync(Guid id)
        {
            var result = await _repository.GetCustomerAsync(id);
            return result.Match<ActionResult>(
                customer => Ok(customer),
                invalidId => NotFound(invalidId.Message)
            );
        }
    }
}
