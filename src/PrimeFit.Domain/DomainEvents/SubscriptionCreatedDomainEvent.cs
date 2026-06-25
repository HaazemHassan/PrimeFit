using PrimeFit.Domain.Primitives.PrimeFit.Domain.Primitives;

namespace PrimeFit.Domain.DomainEvents
{
    public sealed record SubscriptionCreatedDomainEvent(
        int SubscriptionId,
        int UserId,
        int BranchId,
        string BranchName) : IDomainEvent;
}
