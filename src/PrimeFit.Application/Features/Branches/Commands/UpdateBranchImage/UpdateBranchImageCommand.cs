using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateBranchImage
{

    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    public class UpdateBranchImageCommand : IRequest<ErrorOr<UpdateBranchImageCommandResponse>>, IAuthorizedRequest
    {
        public int ImageId { get; set; }
        public int BranchId { get; set; }
        public Stream ImageStream { get; set; } = default!;
    }
}
