using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Commands.DeleteBranchImage
{
    [Authorize(Roles = [UserRole.Owner])]
    public class DeleteBranchImageCommand : IRequest<ErrorOr<Success>>, IAuthorizedRequest
    {
        public int BranchId { get; set; }
        public int ImageId { get; set; }

    }
}
