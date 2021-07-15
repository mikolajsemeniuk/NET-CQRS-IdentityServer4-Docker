using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using common.Responses;
using customer.read.Payloads;
using OneOf;

namespace customer.read.Interfaces
{
    public interface ICustomerRepository
    {
        Task<OneOf<CustomerPayload, CustomerInvalidId>> GetCustomerAsync(Guid id);
        Task<IEnumerable<CustomerPayload>> GetCustomersAsync();
    }
}