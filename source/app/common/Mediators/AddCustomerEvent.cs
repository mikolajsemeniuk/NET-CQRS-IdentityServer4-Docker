using System;

namespace common.Mediators
{
    public record AddCustomerMediator(Guid CustomerId, string Name, string Surname);
}