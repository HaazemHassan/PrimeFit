using ErrorOr;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Security.Contracts
{
    public interface IBranchAuthorizationService
    {
        Task<ErrorOr<Success>> AuthorizeAsync(
            int branchId,
            Permission requiredPermission,
            CancellationToken cancellationToken = default);
    }
}
