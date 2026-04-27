using ErrorOr;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Security.Caching;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure.Cashing;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Infrastructure.Data;

namespace PrimeFit.Infrastructure.Security
{
    internal class BranchAuthorizationService(AppDbContext dbContext,
              ICurrentUserService currentUserService,
              ICacheService cacheService) : IBranchAuthorizationService
    {
        public async Task<ErrorOr<Success>> AuthorizeAsync(
            int branchId,
            Permission requiredPermission,
            CancellationToken cancellationToken = default)
        {

            if (currentUserService.UserType == UserType.SuperAdmin)
            {
                return Result.Success;
            }

            //If the user has a global permission, grant access immediately without further checks
            //( Currently no system roles )
            if (currentUserService.HasPermission(requiredPermission))
            {
                return Result.Success;
            }

            var currentUserId = currentUserService.UserId;

            if (currentUserId == null)
            {
                return Error.Unauthorized(description: "User is not authenticated");
            }

            var cacheKey = SecurityCache.BranchAuthKey(currentUserId.Value, branchId);
            var cacheTag = SecurityCache.BranchAuthTag(branchId);

            var authorizationInfo = await cacheService.GetOrCreateAsync(
                cacheKey,
                async ct =>
                {
                    return await dbContext.Branches
                        .AsNoTracking()
                        .Where(b => b.Id == branchId)
                        .Select(b => new BranchAccessInfo
                        {
                            IsOwner = b.OwnerId == currentUserId,
                            EmployeePermissions = dbContext.Employees
                                .Where(e => e.BranchId == branchId
                                          && e.UserId == currentUserId && e.User.UserType == UserType.Employee
                                          && !e.IsDeleted)
                                .SelectMany(e => e.Role.Permissions.Select(p => p.Permission))
                                .ToList()
                        }).FirstOrDefaultAsync(ct);
                },
                expiration: TimeSpan.FromHours(1),
                tags: [cacheTag],
                ct: cancellationToken);

            if (authorizationInfo is null)
            {
                return Error.NotFound(ErrorCodes.Branch.BranchNotFound, "Branch not found.");
            }

            if (authorizationInfo.IsOwner || authorizationInfo.EmployeePermissions.Contains(requiredPermission))
            {
                return Result.Success;
            }

            return Error.Forbidden(ErrorCodes.Authorization.NotAllowed, "Branch not found.");
        }
    }

    public class BranchAccessInfo
    {
        public bool IsOwner { get; set; }
        public List<Permission> EmployeePermissions { get; set; } = [];
    }
}
