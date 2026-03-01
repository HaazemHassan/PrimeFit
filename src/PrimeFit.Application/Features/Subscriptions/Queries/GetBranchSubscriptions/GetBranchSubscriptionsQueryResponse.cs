using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetBranchSubscriptions
{
    public class GetBranchSubscriptionsQueryResponse
    {

        public int SubscriptionId { get; set; }
        public string FullName { get; set; }
        public SubscriptionStatus Status { get; set; }

        public int TotalDurationInDays { get; set; }

        public int RemainingDurationInDays { get; set; }

    }
}
