using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Subscriptions.Commands.UnfreezeSubscription
{

    [Authorize(Roles = [UserRole.Owner])]
    public class UnfreezeSubscriptionCommand : IRequest<ErrorOr<Success>>, IAuthorizedRequest
    {
        public int SubscriptionId { get; set; }
    }
}
