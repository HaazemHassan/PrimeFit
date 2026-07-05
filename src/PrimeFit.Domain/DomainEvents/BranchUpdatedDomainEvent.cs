using PrimeFit.Domain.Primitives;

namespace PrimeFit.Domain.DomainEvents
{
    public sealed record BranchUpdatedDomainEvent(int BranchId, int OwnerId, string BranchName) : IDomainEvent;
}
