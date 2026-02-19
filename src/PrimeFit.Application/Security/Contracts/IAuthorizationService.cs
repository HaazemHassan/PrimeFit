using ErrorOr;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Security.Contracts
{
    public interface IAuthorizationService
    {
        ErrorOr<Success> AuthorizeCurrentUser<TRequest>(
            TRequest request,
            List<UserRole> requiredRoles,
            List<Permission> requiredPermissions,
            List<string> requiredPolicies);
    }
}
