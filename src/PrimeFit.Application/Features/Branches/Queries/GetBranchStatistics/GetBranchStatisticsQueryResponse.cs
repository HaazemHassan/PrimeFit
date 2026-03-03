namespace PrimeFit.Application.Features.Branches.Queries.GetBranchStatistics
{
    public class GetBranchStatisticsQueryResponse
    {
        public int Id { get; set; }
        public int NewSubscriptionsCount { get; set; }
        public int ExpiredSubscriptionsCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public int ChecksInCout { get; set; }


    }
}
