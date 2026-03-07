using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetMySubscriptions
{
    public class GetMySubscriptionsQueryResponse
    {
        public int SubscriptionId { get; set; }
        public string BranchName { get; set; } = null!;
        public string? BranchLogoUrl { get; set; }
        public string PackageName { get; set; } = null!;
        public SubscriptionStatus Status { get; set; }
        public int TotalDurationInDays { get; set; }
        public int DaysLeft { get; set; }
    }
}
