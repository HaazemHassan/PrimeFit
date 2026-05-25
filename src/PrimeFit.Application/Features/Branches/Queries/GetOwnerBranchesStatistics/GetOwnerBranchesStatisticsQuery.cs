using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Queries.GetOwnerBranchesStatistics
{
    public class GetOwnerBranchesStatisticsQuery : IRequest<ErrorOr<GetOwnerBranchesStatisticsQueryResponse>>
    {
        public TimePeriod? TimePeriod { get; set; }
    }
}
