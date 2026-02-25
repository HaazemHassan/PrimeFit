using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Specifications.Branches;
using PrimeFit.Domain.Repositories;
using PrimeFit.Domain.Specifications.Users;

namespace PrimeFit.Application.Features.Users.Queries.GetUsersPaginated
{
    public class GetMyBranchesQueryHandler : IRequestHandler<GetMyBranchesQuery, ErrorOr<PaginatedResult<GetMyBranchesQueryResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetMyBranchesQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<PaginatedResult<GetMyBranchesQueryResponse>>> Handle(GetMyBranchesQuery request, CancellationToken cancellationToken)
        {
            int ownerId = _currentUserService.UserId!.Value;

            var dataSpec = new BranchesPaginatedForOwnerSpec(ownerId, request.PageNumber, request.PageSize, request.Search, request.SortBy);
            var countSpec = new BranchesSearchSpec(ownerId, request.Search);

            var items = await _unitOfWork.Branches.ListAsync<GetMyBranchesQueryResponse>(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Branches.CountAsync(countSpec, cancellationToken);

            return new PaginatedResult<GetMyBranchesQueryResponse>(items, totalCount, request.PageNumber, request.PageSize);
        }


    }
}
