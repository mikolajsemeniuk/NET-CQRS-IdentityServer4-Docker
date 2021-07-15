using System;
using System.Threading.Tasks;
using common.Mediators;
using customer.read.Data;
using customer.read.Entities;
using MassTransit;
using MongoDB.Driver;

namespace customer.read.Consumers
{
    public class RemoveCustomerConsumer : IConsumer<RemoveCustomerMediator>
    {
        private readonly DataContext _context;
        private readonly FilterDefinitionBuilder<CustomerEntity> _filter = Builders<CustomerEntity>.Filter;

        public RemoveCustomerConsumer(DataContext context)
        {
            _context = context;
        }
        public async Task Consume(ConsumeContext<RemoveCustomerMediator> context)
        {
            var filter = _filter.Eq(customer => customer.CustomerId, context.Message.CustomerId);

            var customer = await _context.Customers.Find(filter).FirstOrDefaultAsync();

            await _context.Customers.DeleteOneAsync(filter);
        }
    }
}