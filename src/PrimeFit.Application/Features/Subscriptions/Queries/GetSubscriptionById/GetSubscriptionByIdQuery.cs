using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetSubscriptionById
{

    [Authorize(Roles = [UserRole.Owner])]
    public class GetSubscriptionByIdQuery : IRequest<ErrorOr<GetSubscriptionByIdQueryResponse>>, IAuthorizedRequest
    {
        public int SubscriptionId { get; set; }

    }
}
