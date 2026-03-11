using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Subscriptions.Commands.AddSubscription
{

    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    public class AddSubscriptionCommand : IRequest<ErrorOr<AddSubscriptionCommandResponse>>, IAuthorizedRequest
    {
        public string Email { get; set; }
        public int PackageId { get; set; }
        public int BranchId { get; set; }
    }
}
