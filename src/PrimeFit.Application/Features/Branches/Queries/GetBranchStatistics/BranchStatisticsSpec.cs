using Ardalis.Specification;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchStatistics
{
    public class BranchStatisticsSpec : Specification<Branch, GetBranchStatisticsQueryResponse>
    {
        public BranchStatisticsSpec(
            int branchId,
            DateTimeOffset? startDate = null,
            DateTimeOffset? now = null)
        {
            now ??= DateTimeOffset.UtcNow;

            Query
                .Where(b => b.Id == branchId)
                .Select(b => new GetBranchStatisticsQueryResponse
                {
                    Id = b.Id,

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