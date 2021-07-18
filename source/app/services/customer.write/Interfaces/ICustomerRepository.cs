using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using common.Responses;
using customer.write.Entities;
using customer.write.Inputs;
using OneOf;

namespace customer.write.Interfaces
{
    public interface ICustomerRepository
    {
        Task<OneOf<CustomerCreated>> AddCustomerAsync(CustomerInput input);
        Task<OneOf<CustomerRemoved, CustomerInvalidId>> RemoveCustomerAsync(Guid id);
        Task<OneOf<CustomerUpdated, CustomerInvalidId>> UpdateCustomerAsync(Guid id, CustomerInput input);
    }
}