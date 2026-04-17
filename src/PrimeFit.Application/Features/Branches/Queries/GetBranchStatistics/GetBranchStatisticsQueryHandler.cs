using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Enums;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchStatistics
{
    public class GetBranchStatisticsQueryHandler : IRequestHandler<GetBranchStatisticsQuery, ErrorOr<GetBranchStatisticsQueryResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IBranchAuthorizationService _branchAuthorizationService;
        public GetBranchStatisticsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IDateTimeProvider dateTimeProvider, IBranchAuthorizationService branchAuthorizationService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
            _branchAuthorizationService = branchAuthorizationService;
        }



        public async Task<ErrorOr<GetBranchStatisticsQueryResponse>> Handle(GetBranchStatisticsQuery request, CancellationToken cancellationToken)
        {
            var authResult = await _branchAuthorizationService.AuthorizeAsync(
                request.BranchId,
                Permission.BranchDetailsRead,
                cancellationToken);

            if (authResult.IsError)
            {
                return authResult.Errors;
            }


            var now = _dateTimeProvider.GetTimeZoneNow("Africa/Cairo");

            DateTimeOffset? startDate = null;
            if (request.TimePeriod.HasValue)
            {
                startDate = GetStartDate(request.TimePeriod.Value, now);
            }

            var statisticsSpec = new BranchStatisticsSpec(request.BranchId, startDate, now);

            var branchResponse = await _unitOfWork.Branches.FirstOrDefaultAsync(statisticsSpec, cancellationToken);
            if (branchResponse is null)
            {
                return Error.NotFound(
                    ErrorCodes.Branch.BranchNotFound, "Branch not found");
            }


            return branchResponse;

        }



        private DateTimeOffset GetStartDate(TimePeriod period, DateTimeOffset now)
        {
            return period switch
            {
                TimePeriod.Today => now.Date,
                TimePeriod.ThisWeek => now.AddDays(-(int)now.DayOfWeek),
                TimePeriod.ThisMonth => new DateTimeOffset(now.Year, now.Month, 1, 0, 0, 0, TimeSpan.Zero),
                _ => now.Date
            };
        }
    }
}
