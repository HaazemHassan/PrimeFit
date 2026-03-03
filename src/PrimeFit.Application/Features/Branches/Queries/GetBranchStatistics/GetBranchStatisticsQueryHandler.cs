using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Enums;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Specifications.Branches;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchStatistics
{
    public class GetBranchStatisticsQueryHandler : IRequestHandler<GetBranchStatisticsQuery, ErrorOr<GetBranchStatisticsQueryResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly TimeProvider _timeProvider;
        public GetBranchStatisticsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, TimeProvider timeProvider)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _timeProvider = timeProvider;
        }



        public async Task<ErrorOr<GetBranchStatisticsQueryResponse>> Handle(GetBranchStatisticsQuery request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.UserId!.Value;
            var branchForOwner = new BranchForOwnerSpec(request.BranchId, userId);

            var isOwner = await _unitOfWork.Branches.AnyAsync(branchForOwner, cancellationToken);
            if (!isOwner)
            {

                return Error.NotFound(
                    ErrorCodes.Branch.BranchNotFound, "Branch not found");

            }

            var startDate = GetStartDate(request.TimePeriod);
            var now = _timeProvider.GetUtcNow();

            var statisticsSpec = new BranchStatisticsSpec(request.BranchId, startDate, now);

            var branchResponse = await _unitOfWork.Branches.FirstOrDefaultAsync(statisticsSpec, cancellationToken);
            if (branchResponse is null)
            {
                return Error.NotFound(
                    ErrorCodes.Branch.BranchNotFound, "Branch not found");
            }


            return branchResponse;

        }



        private DateTimeOffset GetStartDate(TimePeriod period)
        {
            var now = DateTimeOffset.UtcNow;
            return period switch
            {
                TimePeriod.Today => now.Date,
                TimePeriod.ThisWeek => now.AddDays(-(int)now.DayOfWeek), // بداية الأسبوع
                TimePeriod.ThisMonth => new DateTimeOffset(now.Year, now.Month, 1, 0, 0, 0, TimeSpan.Zero),
                _ => now.Date
            };
        }
    }
}
