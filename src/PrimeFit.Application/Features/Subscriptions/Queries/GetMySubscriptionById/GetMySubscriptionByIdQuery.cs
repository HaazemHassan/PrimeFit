using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetMySubscriptionById
{
    [Authorize]
    public class GetMySubscriptionByIdQuery : IRequest<ErrorOr<GetMySubscriptionByIdResponse>>, IAuthorizedRequest
    {
        public int SubscriptionId { get; set; }
    }
}