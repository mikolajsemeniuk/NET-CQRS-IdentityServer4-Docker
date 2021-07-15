using System;

namespace customer.read.Payloads
{
    public class CustomerPayload
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}