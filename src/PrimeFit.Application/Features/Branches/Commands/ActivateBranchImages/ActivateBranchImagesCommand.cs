using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Commands.ActivateBranchImages
{
    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    [BranchAuthorize(BranchPermissions = [Permission.BranchImagesWrite])]
    public class ActivateBranchImagesCommand : IRequest<ErrorOr<Success>>, IBranchAuthorizedRequest
    {
        public int BranchId { get; set; }
        public List<int> Images { get; set; } = new List<int>();
    }
}
