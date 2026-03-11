using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetMySubscriptions
{
    [Authorize]
    public class GetMySubscriptionsQuery : PaginatedQuery, IRequest<ErrorOr<PaginatedResult<GetMySubscriptionsQueryResponse>>>, IAuthorizedRequest
    {
        public SubscriptionStatus? Status { get; set; }
    }
}
