using System;
using System.Threading.Tasks;
using common.Mediators;
using customer.read.Data;
using customer.read.Entities;
using MassTransit;
using MongoDB.Driver;

namespace customer.read.Consumers
{
    public class AddCustomerConsumer : IConsumer<AddCustomerMediator>
    {
        private readonly DataContext _context;
        private readonly FilterDefinitionBuilder<CustomerEntity> _filter = Builders<CustomerEntity>.Filter;

        public AddCustomerConsumer(DataContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<AddCustomerMediator> context)
        {
            var customer = new CustomerEntity
            {
                CustomerId = context.Message.CustomerId,
                Name = context.Message.Name,
                Surname = context.Message.Surname
            };
            await _context.Customers.InsertOneAsync(customer);
        }
    }
}