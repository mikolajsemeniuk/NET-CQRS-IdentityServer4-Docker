using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using common.Mediators;
using common.Responses;
using customer.write.Entities;
using customer.write.Inputs;
using customer.write.Interfaces;
using LanguageExt;
using MassTransit;
using MongoDB.Driver;
using OneOf;

namespace customer.write.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IPublishEndpoint _endpoint;
        private readonly IMapper _mapper;
        private readonly IMongoDatabase _context;
        private readonly FilterDefinitionBuilder<CustomerEntity> _filter = Builders<CustomerEntity>.Filter;

        public CustomerRepository(IMongoDatabase database, IPublishEndpoint endpoint, IMapper mapper)
        {
            _endpoint = endpoint;
            _mapper = mapper;
            _context = database;
        }

        public async Task<OneOf<CustomerCreated>> AddCustomerAsync(CustomerInput input)
        {
            var customer = new CustomerEntity();
            _mapper.Map(input, customer);

            await _context.GetCollection<CustomerEntity>("customers").InsertOneAsync(customer);
            await _endpoint.Publish(new AddCustomerMediator(customer.CustomerId, customer.Name, customer.Surname));
            return new CustomerCreated();
        }

        public async Task<OneOf<CustomerRemoved, CustomerInvalidId>> RemoveCustomerAsync(Guid id)
        {
            var filter = _filter.Eq(customer => customer.CustomerId, id);

            var customer = await _context.GetCollection<CustomerEntity>("customers").Find(filter).FirstOrDefaultAsync();
            if (customer is null)
                return new CustomerInvalidId();

            await _context.GetCollection<CustomerEntity>("customers").DeleteOneAsync(filter);
            await _endpoint.Publish(new RemoveCustomerMediator(customer.CustomerId, customer.Name, customer.Surname));
            return new CustomerRemoved();
        }

        public async Task<OneOf<CustomerUpdated, CustomerInvalidId>> UpdateCustomerAsync(Guid id, CustomerInput input)
        {
            var filter = _filter.Eq(customer => customer.CustomerId, id);
            var customer = await _context.GetCollection<CustomerEntity>("customers")
                .Find(filter)
                .FirstOrDefaultAsync();

            if (customer is null)
                return new CustomerInvalidId();

            _mapper.Map(input, customer);

            await _context.GetCollection<CustomerEntity>("customers").ReplaceOneAsync(filter, customer);
            await _endpoint.Publish(new UpdateCustomerMediator(customer.CustomerId, customer.Name, customer.Surname));
            return new CustomerUpdated();
        }
    }
}