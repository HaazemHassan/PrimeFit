using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetSubscriptionAttendanceHistory.Specs
{
    internal class SubscriptionAttendanceTimelineSpec : Specification<CheckIn, DateTimeOffset>
    {
        public SubscriptionAttendanceTimelineSpec(int subscriptionId)
        {
            Query.Where(c => c.SubscriptionId == subscriptionId)
                 .Select(c => c.CreatedAt);
        }
    }
}
