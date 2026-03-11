using ErrorOr;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Infrastructure.Data;

namespace PrimeFit.Infrastructure.Security
{
    internal class BranchAuthorizationService(AppDbContext dbContext,
              ICurrentUserService currentUserService) : IBranchAuthorizationService
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

            var authorized = await dbContext.Branches
                .AsNoTracking()
                .Where(b => b.Id == branchId)
                .Select(b => new
                {
                    IsOwner = b.OwnerId == currentUserId,
                    HasEmployeePermission = dbContext.Employees
                        .Any(e => e.BranchId == branchId
                                  && e.UserId == currentUserId && e.User.UserType == UserType.Employee
                                  && !e.IsDeleted
                                  && e.Role.Permissions.Any(p => p.Permission == requiredPermission))

                }).FirstOrDefaultAsync(cancellationToken);


            if (authorized is null)
            {
                return Error.NotFound(ErrorCodes.Branch.BranchNotFound, "Branch not found.");
            }

            if (authorized.IsOwner || authorized.HasEmployeePermission)
            {
                return Result.Success;
            }

            return Error.Forbidden(ErrorCodes.Authorization.NotAllowed, "You do not have the required access.");
        }
    }
}
