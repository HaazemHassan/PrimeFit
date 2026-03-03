using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Enums;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchStatistics
{

    [Authorize(Roles = [UserRole.Owner])]
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
