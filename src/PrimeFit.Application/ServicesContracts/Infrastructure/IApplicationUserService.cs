using ErrorOr;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.ServicesContracts.Infrastructure
{
    public interface IApplicationUserService
    {
        public Task<ErrorOr<int>> AddUser(DomainUser user, string password, UserRole? userRole = default, CancellationToken ct = default);
        public Task<bool> isAssociatedWithDomainUser(int domainUserId, CancellationToken ct = default);
        public Task<ErrorOr<Success>> UpdateLinkedUser(int domainUserId, string? email, string? phoneNumber, CancellationToken ct = default);



    }

}