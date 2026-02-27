using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Subscriptions.Commands.AddSubscription
{

    [Authorize(Roles = [UserRole.Owner])]
    public class AddSubscriptionCommand : IRequest<ErrorOr<AddSubscriptionCommandResponse>>, IAuthorizedRequest
    {
        public string Email { get; set; }
        public int PackageId { get; set; }
        public int BranchId { get; set; }
    }
}
