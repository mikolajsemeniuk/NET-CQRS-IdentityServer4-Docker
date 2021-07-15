using customer.read.Entities;
using MongoDB.Driver;

namespace customer.read.Data
{
    public class DataContext
    {
        public readonly IMongoCollection<CustomerEntity> Customers;

        public DataContext(IMongoDatabase database) =>
            Customers = database.GetCollection<CustomerEntity>("customers");
    }
}