using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Application.Specifications.Subscriptions;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetMySubscriptions
{
    public class GetMySubscriptionsQueryHandler : IRequestHandler<GetMySubscriptionsQuery, ErrorOr<PaginatedResult<GetMySubscriptionsQueryResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetMySubscriptionsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<PaginatedResult<GetMySubscriptionsQueryResponse>>> Handle(GetMySubscriptionsQuery request, CancellationToken cancellationToken)
        {
            int userId = _currentUserService.UserId!.Value;

            var dataSpec = new UserSubscriptionsSpec(userId, request.Status, request.PageNumber, request.PageSize);

            var subscriptions = await _unitOfWork.Subscriptions.ListAsync(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Subscriptions.CountAsync(dataSpec, cancellationToken);

            var now = _dateTimeProvider.GetNow("Africa/Cairo");

            var data = subscriptions.Select(s =>
            {
                int daysLeft = s.GetRemainingDays(now);

                return new GetMySubscriptionsQueryResponse
                {
                    SubscriptionId = s.Id,
                    BranchName = s.Branch.Name,
                    BranchLogoUrl = s.Branch.Logo?.Url,
                    PackageName = s.Package.Name,
                    Status = s.Status,
                    TotalDurationInDays = s.DurationInMonths * 30,
                    DaysLeft = daysLeft
                };
            }).ToList();

            return new PaginatedResult<GetMySubscriptionsQueryResponse>(data, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
