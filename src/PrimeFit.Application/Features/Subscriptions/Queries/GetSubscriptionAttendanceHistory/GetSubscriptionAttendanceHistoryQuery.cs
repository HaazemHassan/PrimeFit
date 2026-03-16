using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetSubscriptionAttendanceHistory
{
    [Authorize()]
    public class GetSubscriptionAttendanceHistoryQuery : IRequest<ErrorOr<GetSubscriptionAttendanceHistoryResponse>>, IAuthorizedRequest
    {
        public int SubscriptionId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
    }
}