using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateBranchStatus
{
    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    [BranchAuthorize(BranchPermissions = [Permission.BranchDetailsWrite])]
    public class UpdateBranchStatusCommand : IRequest<ErrorOr<Success>>, IBranchAuthorizedRequest
    {
        public int BranchId { get; set; }
        public BranchStatus BranchStatus { get; set; }
    }
}
