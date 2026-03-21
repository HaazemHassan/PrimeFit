using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Branches.Commands.ActivateBranchImages
{
    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    public class ActivateBranchImagesCommand : IRequest<ErrorOr<Success>>, IAuthorizedRequest
    {
        public int BranchId { get; set; }
        public List<int> Images { get; set; } = new List<int>();
    }
}
