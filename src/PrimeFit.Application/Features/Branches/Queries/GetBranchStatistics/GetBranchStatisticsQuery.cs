using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Enums;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchStatistics
{

    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    [BranchAuthorize(BranchPermissions = [Permission.BranchDetailsRead])]
    public class GetBranchStatisticsQuery : IRequest<ErrorOr<GetBranchStatisticsQueryResponse>>, IBranchAuthorizedRequest
    {

        public int BranchId { get; set; }
        public TimePeriod? TimePeriod { get; set; }


        public GetBranchStatisticsQuery()
        {
        }

        public GetBranchStatisticsQuery(int branchId)
        {
            BranchId = branchId;
        }


    }
}
