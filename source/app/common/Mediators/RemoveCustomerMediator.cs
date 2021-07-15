using System;

namespace common.Mediators
{
    public record RemoveCustomerMediator(Guid CustomerId, string Name, string Surname);
}