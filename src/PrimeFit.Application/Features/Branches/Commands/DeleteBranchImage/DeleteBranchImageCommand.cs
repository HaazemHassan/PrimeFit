using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Branches.Commands.DeleteBranchImage
{

    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    public class DeleteBranchImageCommand : IRequest<ErrorOr<Success>>, IAuthorizedRequest
    {
        public int BranchId { get; set; }
        public int ImageId { get; set; }

    }
}
