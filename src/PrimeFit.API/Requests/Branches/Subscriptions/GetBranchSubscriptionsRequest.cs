using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.API.Requests.Branches.Subscriptions
{
    public class GetBranchSubscriptionsRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public SubscriptionStatus? SubscriptionStatus { get; set; }

    }
}
