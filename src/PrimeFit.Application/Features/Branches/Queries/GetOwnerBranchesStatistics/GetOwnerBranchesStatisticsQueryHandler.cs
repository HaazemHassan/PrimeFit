using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Helpers;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Branches.Queries.GetOwnerBranchesStatistics
{
    public class GetOwnerBranchesStatisticsQueryHandler : IRequestHandler<GetOwnerBranchesStatisticsQuery, ErrorOr<GetOwnerBranchesStatisticsQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetOwnerBranchesStatisticsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<GetOwnerBranchesStatisticsQueryResponse>> Handle(GetOwnerBranchesStatisticsQuery request, CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.GetTimeZoneNow("Africa/Cairo");

            DateTimeOffset? startDate = null;
            if (request.TimePeriod.HasValue)
            {
                startDate = DateTimeHelper.GetStartDate(request.TimePeriod.Value, now);
            }

            var userId = _currentUserService.UserId;

            if (userId == null)
            {
                return Error.Unauthorized();
            }

            var statisticsSpec = new OwnerBranchesStatisticsSpec(userId.Value, startDate, now);

            var branchesStatistics = await _unitOfWork.Branches.ListAsync(statisticsSpec, cancellationToken);

            var aggregateResponse = new GetOwnerBranchesStatisticsQueryResponse
            {
                NewSubscriptionsCount = branchesStatistics.Sum(b => b.NewSubscriptionsCount),
                ExpiredSubscriptionsCount = branchesStatistics.Sum(b => b.ExpiredSubscriptionsCount),
                TotalRevenue = branchesStatistics.Sum(b => b.TotalRevenue)
            };

            return aggregateResponse;
        }
    }
}
