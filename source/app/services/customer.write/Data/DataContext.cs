using customer.write.Entities;
using MongoDB.Driver;

namespace customer.write.Data
{
    public class DataContext
    {
        public readonly IMongoCollection<CustomerEntity> Customers;

        public DataContext(IMongoDatabase database) =>
            Customers = database.GetCollection<CustomerEntity>("customers");
    }
}