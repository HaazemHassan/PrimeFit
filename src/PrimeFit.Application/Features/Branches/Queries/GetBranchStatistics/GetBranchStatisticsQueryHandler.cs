using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Enums;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.RepositoriesContracts;

using PrimeFit.Application.Common.Helpers;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchStatistics
{
    public class GetBranchStatisticsQueryHandler : IRequestHandler<GetBranchStatisticsQuery, ErrorOr<GetBranchStatisticsQueryResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;
        public GetBranchStatisticsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }



        public async Task<ErrorOr<GetBranchStatisticsQueryResponse>> Handle(GetBranchStatisticsQuery request, CancellationToken cancellationToken)
        {

            var now = _dateTimeProvider.GetTimeZoneNow("Africa/Cairo");

            DateTimeOffset? startDate = null;
            if (request.TimePeriod.HasValue)
            {
                startDate = DateTimeHelper.GetStartDate(request.TimePeriod.Value, now);
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
    }
}
