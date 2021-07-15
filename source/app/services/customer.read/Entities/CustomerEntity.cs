using System;
using MongoDB.Bson.Serialization.Attributes;

namespace customer.read.Entities
{
    public class CustomerEntity
    {
        [BsonId]
        public Guid CustomerId { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("surname")]
        public string Surname { get; set; }
    }
}