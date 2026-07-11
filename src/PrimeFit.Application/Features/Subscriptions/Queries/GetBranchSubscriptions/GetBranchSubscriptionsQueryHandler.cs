using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Application.Specifications.Subscriptions;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetBranchSubscriptions
{
    public class GetBranchSubscriptionsQueryHandler : IRequestHandler<GetBranchSubscriptionsQuery, ErrorOr<PaginatedResult<GetBranchSubscriptionsQueryResponse>>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetBranchSubscriptionsQueryHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }



        public async Task<ErrorOr<PaginatedResult<GetBranchSubscriptionsQueryResponse>>> Handle(GetBranchSubscriptionsQuery request, CancellationToken cancellationToken)
        {

            var dataSpec = new BranchSubscriptionsPaginatedSpec(request.BranchId, request.SubscriptionStatus, request.Search, request.PageNumber, request.PageSize);
            var countSpec = new BranchSubscriptionsFilteredSpec(request.BranchId, request.SubscriptionStatus, request.Search);

            var subscriptions = await _unitOfWork.Subscriptions.ListAsync(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Subscriptions.CountAsync(countSpec, cancellationToken);

            var dtos = subscriptions.Select(s => new GetBranchSubscriptionsQueryResponse
            {
                SubscriptionId = s.Id,
                FullName = s.User.FullName,
                Status = s.Status,
                TotalDurationInDays = s.DurationInMonths * 30,
                RemainingDurationInDays = s.GetRemainingDays(_dateTimeProvider.UtcNow)
            }).ToList();

            return new PaginatedResult<GetBranchSubscriptionsQueryResponse>(dtos, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
