using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.Specifications.Subscriptions;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetBranchSubscriptions
{
    public class GetBranchSubscriptionsQueryHandler : IRequestHandler<GetBranchSubscriptionsQuery, ErrorOr<PaginatedResult<GetBranchSubscriptionsQueryResponse>>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly TimeProvider _timeProvider;
        private readonly IBranchAuthorizationService _branchAuthorizationService;

        public GetBranchSubscriptionsQueryHandler(IUnitOfWork unitOfWork, TimeProvider timeProvider, IBranchAuthorizationService branchAuthorizationService)
        {
            _unitOfWork = unitOfWork;
            _timeProvider = timeProvider;
            _branchAuthorizationService = branchAuthorizationService;
        }



        public async Task<ErrorOr<PaginatedResult<GetBranchSubscriptionsQueryResponse>>> Handle(GetBranchSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            var authResult = await _branchAuthorizationService.AuthorizeAsync(request.BranchId, Permission.SubscriptionsView, cancellationToken);
            if (authResult.IsError)
            {
                return authResult.Errors;
            }

            var dataSpec = new BranchSubscriptionsPaginatedSpec(request.BranchId, request.SubscriptionStatus, request.Search, request.PageNumber, request.PageSize);
            var countSpec = new BranchSubscriptionSearchSpec(request.BranchId, request.Search);

            var subscriptions = await _unitOfWork.Subscriptions.ListAsync(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Subscriptions.CountAsync(countSpec, cancellationToken);

            var dtos = subscriptions.Select(s => new GetBranchSubscriptionsQueryResponse
            {
                SubscriptionId = s.Id,
                FullName = s.User.FullName,
                Status = s.Status,
                TotalDurationInDays = s.DurationInMonths * 30,
                RemainingDurationInDays = s.GetRemainingDays(_timeProvider.GetUtcNow())
            }).ToList();

            return new PaginatedResult<GetBranchSubscriptionsQueryResponse>(dtos, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
