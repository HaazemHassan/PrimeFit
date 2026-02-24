using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Commands.UpdateBranchImage
{
    [Authorize(Roles = [UserRole.Owner])]
    public class UpdateBranchImageCommand : IRequest<ErrorOr<UpdateBranchImageCommandResponse>>, IAuthorizedRequest
    {
        public int ImageId { get; set; }
        public int BranchId { get; set; }
        public Stream ImageStream { get; set; } = default!;
    }
}
