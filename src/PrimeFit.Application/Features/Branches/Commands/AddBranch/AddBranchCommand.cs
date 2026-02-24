using ErrorOr;
using MediatR;
using PrimeFit.Application.Features.Branches.Commands.AddBranch;
using PrimeFit.Application.Features.Branches.Commands.AddBranchImage;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Commands.AddBranchBussinessDetails
{
    [Authorize(Roles = [UserRole.Owner])]
    public class AddBranchCommand : IRequest<ErrorOr<AddBranchCommandResponse>>, IAuthorizedRequest
    {

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public BranchType BranchType { get; set; }


    }
}
