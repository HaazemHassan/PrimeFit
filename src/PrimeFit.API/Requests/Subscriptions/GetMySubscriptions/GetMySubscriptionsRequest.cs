using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Api.Requests.Subscriptions.GetMySubscriptions
{
    public class GetMySubscriptionsRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public SubscriptionStatus? Status { get; set; }
    }
}
