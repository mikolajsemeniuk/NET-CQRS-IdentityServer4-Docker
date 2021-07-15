using System;

namespace common.Mediators
{
    public record UpdateCustomerMediator(Guid CustomerId, string Name, string Surname);
}