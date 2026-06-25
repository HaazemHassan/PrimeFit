using PrimeFit.Domain.Primitives.PrimeFit.Domain.Primitives;

namespace PrimeFit.Domain.DomainEvents
{
    public sealed record UserRegisteredDomainEvent(string Email) : IDomainEvent;
}
