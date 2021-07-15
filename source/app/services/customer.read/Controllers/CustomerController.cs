using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using customer.read.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace customer.read.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _repository;
        public CustomerController(ICustomerRepository repository)
        {
            _repository = repository;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetCustomersAsync() =>
            Ok(await _repository.GetCustomersAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustomerAsync(Guid id)
        {
            var result = await _repository.GetCustomerAsync(id);
            return result.Match<ActionResult>(
                customer => Ok(customer),
                invalidId => NotFound(invalidId.Message)
            );
        }
    }
}
