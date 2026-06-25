using PrimeFit.Domain.Primitives.PrimeFit.Domain.Primitives;

namespace PrimeFit.Domain.DomainEvents
{
    public sealed record BranchPackagesUpdatedDomainEvent(int BranchId, int OwnerId, string BranchName) : IDomainEvent;
}
