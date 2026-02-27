using ErrorOr;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Domain.ServicesContracts
{
    public interface ISubscriptionDomainService
    {

        public Task<ErrorOr<Subscription>> CreateSubscriptionAsync(DomainUser user, Branch branch, Package package, CancellationToken ct = default);
    }
}
