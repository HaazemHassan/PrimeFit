using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Commands.CreateBranch
{

    [Authorize(UserTypes = [UserType.PartnerAdmin])]
    public class CreateBranchCommand : IRequest<ErrorOr<CreateBranchCommandResponse>>, IAuthorizedRequest
    {

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public BranchType BranchType { get; set; }


    }
}
