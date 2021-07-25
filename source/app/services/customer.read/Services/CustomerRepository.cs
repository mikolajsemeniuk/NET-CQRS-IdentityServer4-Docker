using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using common.Responses;
using customer.read.Data;
using customer.read.Entities;
using customer.read.Interfaces;
using customer.read.Payloads;
using MongoDB.Driver;
using OneOf;

namespace customer.read.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private const string Customers = "customers";
        private readonly IMongoDatabase _context;
        private readonly FilterDefinitionBuilder<CustomerEntity> _filter = Builders<CustomerEntity>.Filter;

        public CustomerRepository(IMongoDatabase context) =>
            _context = context;

        public async Task<IEnumerable<CustomerPayload>> GetCustomersAsync() =>
            await _context.GetCollection<CustomerEntity>(Customers)
                .Find(_filter.Empty)
                .Project(customer => new CustomerPayload
                {
                    CustomerId = customer.CustomerId,
                    Name = customer.Name,
                    Surname = customer.Surname
                })
                .ToListAsync();

        public async Task<OneOf<CustomerPayload, CustomerInvalidId>> GetCustomerAsync(Guid id)
        {
            var customer = await _context.GetCollection<CustomerEntity>(Customers)
                .Find(_filter.Eq(customer => customer.CustomerId, id))
                .Project(customer => new CustomerPayload
                {
                    CustomerId = customer.CustomerId,
                    Name = customer.Name,
                    Surname = customer.Surname
                })
                .FirstOrDefaultAsync();

            if (customer == null)
                return new CustomerInvalidId();

            return customer;
        }
    }
}