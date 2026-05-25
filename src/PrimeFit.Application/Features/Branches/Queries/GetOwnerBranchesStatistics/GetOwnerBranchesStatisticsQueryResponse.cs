namespace PrimeFit.Application.Features.Branches.Queries.GetOwnerBranchesStatistics
{
    public class GetOwnerBranchesStatisticsQueryResponse
    {
        public int NewSubscriptionsCount { get; set; }
        public int ExpiredSubscriptionsCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
