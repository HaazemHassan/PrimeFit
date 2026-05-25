using Ardalis.Specification;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Branches.Queries.GetOwnerBranchesStatistics
{
    public class OwnerBranchesStatisticsSpec : Specification<Branch, GetOwnerBranchesStatisticsQueryResponse>
    {
        public OwnerBranchesStatisticsSpec(
            int ownerId,
            DateTimeOffset? startDate = null,
            DateTimeOffset? now = null)
        {
            now ??= DateTimeOffset.UtcNow;

            Query
                .Where(b => b.OwnerId == ownerId)
                .Select(b => new GetOwnerBranchesStatisticsQueryResponse
                {
                    NewSubscriptionsCount = b.Subscriptions
                        .Count(s =>
                            (!startDate.HasValue || s.CreatedAt >= startDate) &&
                            s.CreatedAt <= now),

                    ExpiredSubscriptionsCount = b.Subscriptions
                        .Count(s =>
                            s.Status == SubscriptionStatus.Expired &&
                            (!startDate.HasValue || s.EndDate >= startDate) &&
                            s.EndDate <= now),

                    TotalRevenue = b.Subscriptions
                        .Where(s =>
                            (!startDate.HasValue || s.CreatedAt >= startDate) &&
                            s.CreatedAt <= now)
                        .Sum(s => (decimal?)s.PaidAmount) ?? 0
                });
        }
    }
}
