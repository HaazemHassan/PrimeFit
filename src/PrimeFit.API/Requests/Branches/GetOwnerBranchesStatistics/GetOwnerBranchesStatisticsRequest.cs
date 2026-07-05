using PrimeFit.Application.Common.Enums;

namespace PrimeFit.Api.Requests.Branches.GetOwnerBranchesStatistics {
    public class GetOwnerBranchesStatisticsRequest
    {
        public TimePeriod? TimePeriod { get; set; }
    }
}
