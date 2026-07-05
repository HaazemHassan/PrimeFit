using PrimeFit.Application.Common.Enums;

namespace PrimeFit.Api.Requests.Branches.GetBranchStatistics {
    public class GetBranchStatisticsRequest
    {
        public TimePeriod? TimePeriod { get; set; }
    }
}
