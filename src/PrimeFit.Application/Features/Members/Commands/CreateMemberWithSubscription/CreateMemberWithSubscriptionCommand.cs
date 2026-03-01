using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Members.Commands.CreateMemberWithSubscription
{

    [Authorize(Roles = [UserRole.Owner])]
    public class CreateMemberWithSubscriptionCommand : IRequest<ErrorOr<CreateMemberWithSubscriptionCommandResponse>>, IAuthorizedRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int PackageId { get; set; }
        public int BranchId { get; set; }
    }
}
