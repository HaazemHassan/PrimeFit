using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Enums;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchStatistics
{

    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    public class GetBranchStatisticsQuery : IRequest<ErrorOr<GetBranchStatisticsQueryResponse>>, IAuthorizedRequest
    {

        public int BranchId { get; set; }
        public TimePeriod TimePeriod { get; set; }


        public GetBranchStatisticsQuery()
        {
        }

        public GetBranchStatisticsQuery(int branchId)
        {
            BranchId = branchId;
        }


    }
}
