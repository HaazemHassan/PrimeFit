using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Branches.Queries.GetNearbyBranches
{
    public class GetNearbyBranchesQueryHandler : IRequestHandler<GetNearbyBranchesQuery, ErrorOr<PaginatedResult<GetNearbyBranchesQueryResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetNearbyBranchesQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<PaginatedResult<GetNearbyBranchesQueryResponse>>> Handle(GetNearbyBranchesQuery request, CancellationToken cancellationToken)
        {

            var dataSpec = new NearbyBranchesSpec(request.Latitude,
                request.Longitude,
                request.RadiusInMeters,
                request.PageNumber,
                request.PageSize,
                request.search
             );


            var items = await _unitOfWork.Branches.ListAsync(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Branches.CountAsync(dataSpec, cancellationToken);

            return new PaginatedResult<GetNearbyBranchesQueryResponse>(items, totalCount, request.PageNumber, request.PageSize);
        }


    }
}
