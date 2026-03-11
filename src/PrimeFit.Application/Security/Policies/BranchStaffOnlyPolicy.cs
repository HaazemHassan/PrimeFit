using ErrorOr;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Security.Policies
{
    public class BranchStaffOnlyPolicy : IAuthorizationPolicy
    {
        public string Name => AuthorizationPolicies.BranchStaffOnly;


        private readonly ICurrentUserService _currentUserService;

        public BranchStaffOnlyPolicy(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public ErrorOr<Success> Authorize(object request)
        {
            var userType = _currentUserService.UserType;

            if (userType == UserType.PartnerAdmin || userType == UserType.Employee)
            {
                return Result.Success;

            }

            return Error.Validation(
                 code: ErrorCodes.Authorization.NotAllowed,
                description: "Forbidden");


        }
    }
}