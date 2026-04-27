using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Commands.CreateBranchImage
{
    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    [BranchAuthorize(BranchPermissions = [Permission.BranchImagesWrite])]
    public class AddBranchImageCommand : IRequest<ErrorOr<AddBranchImageCommandResponse>>, IBranchAuthorizedRequest
    {

        public int BranchId { get; set; }
        public Stream ImageStream { get; set; } = default!;
        public BranchImageType ImageType { get; set; }
        public int DisplayOrder { get; set; }

    }
}
