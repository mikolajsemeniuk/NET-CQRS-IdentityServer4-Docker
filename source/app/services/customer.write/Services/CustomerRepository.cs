using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using common.Responses;
using customer.write.Data;
using customer.write.Entities;
using customer.write.Inputs;
using customer.write.Interfaces;
using LanguageExt;
using MongoDB.Driver;
using OneOf;

namespace customer.write.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly FilterDefinitionBuilder<CustomerEntity> _filter = Builders<CustomerEntity>.Filter;

        public CustomerRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // TODO: only for dev
        public async Task<IEnumerable<CustomerEntity>> GetCustomersAsync() =>
            await _context.Customers
                .Find(_filter.Empty)
                .ToListAsync();

        public async Task<OneOf<CustomerCreated>> AddCustomerAsync(CustomerInput input)
        {
            var customer = new CustomerEntity();
            _mapper.Map(input, customer);

            await _context.Customers.InsertOneAsync(customer);
            return new CustomerCreated();
        }

        public async Task<OneOf<CustomerRemoved, CustomerInvalidId>> RemoveCustomerAsync(Guid id)
        {
            var filter = _filter.Eq(customer => customer.CustomerId, id);

            var customer = await _context.Customers.Find(filter).FirstOrDefaultAsync();
            if (customer is null)
                return new CustomerInvalidId();

            await _context.Customers.DeleteOneAsync(filter);
            return new CustomerRemoved();
        }

        public async Task<OneOf<CustomerUpdated, CustomerInvalidId>> UpdateCustomerAsync(Guid id, CustomerInput input)
        {
            var filter = _filter.Eq(customer => customer.CustomerId, id);
            var customer = await _context.Customers
                .Find(filter)
                .FirstOrDefaultAsync();

            if (customer is null)
                return new CustomerInvalidId();

            _mapper.Map(input, customer);

            await _context.Customers.ReplaceOneAsync(filter, customer);
            return new CustomerUpdated();
        }
    }
}