using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Features.Branches.Queries.GetMyBranches;
using PrimeFit.Application.Specifications.Subscriptions;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetBranchSubscriptions
{
    public class GetBranchSubscriptionsQueryHandler : IRequestHandler<GetBranchSubscriptionsQuery, ErrorOr<PaginatedResult<GetBranchSubscriptionsQueryResponse>>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public GetBranchSubscriptionsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }



        public async Task<ErrorOr<PaginatedResult<GetBranchSubscriptionsQueryResponse>>> Handle(GetBranchSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            int ownerId = _currentUserService.UserId!.Value;

            var branch = await _unitOfWork.Branches.GetByIdAsync(request.BranchId, cancellationToken);
            if (branch is null || branch.OwnerId != ownerId)
            {
                return Error.Failure(
                    ErrorCodes.Branch.BranchNotFound, description: "Branch not found");
            }

            var dataSpec = new BranchSubscriptionsPaginatedSpec(request.BranchId, request.SubscriptionStatus, request.Search, request.PageNumber, request.PageSize);
            var countSpec = new BranchSubscriptionSearchSpec(request.BranchId, request.Search);

            var items = await _unitOfWork.Subscriptions.ListAsync<GetBranchSubscriptionsQueryResponse>(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Subscriptions.CountAsync(countSpec, cancellationToken);

            return new PaginatedResult<GetMyBranchesQueryResponse>(items, totalCount, request.PageNumber, request.PageSize);

        }
    }
}
