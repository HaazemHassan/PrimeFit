using Ardalis.Specification;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Features.Branches.Queries.GetBranchesForPublic.Specs;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchesForPublic
{
    public class GetBranchesForPublicQueryHandler : IRequestHandler<GetBranchesForPublicQuery, ErrorOr<PaginatedResult<GetBranchesForPublicQueryResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetBranchesForPublicQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<PaginatedResult<GetBranchesForPublicQueryResponse>>> Handle(GetBranchesForPublicQuery request, CancellationToken cancellationToken)
        {
            ISpecification<Branch, GetBranchesForPublicQueryResponse> dataSpec;

            if (request.Latitude.HasValue && request.Longitude.HasValue)
            {
                dataSpec = new NearbyBranchesSpec(
                    request.Latitude.Value,
                    request.Longitude.Value,
                    request.RadiusInMeters,
                    request.PageNumber,
                    request.PageSize,
                    request.search
                );
            }
            else
            {
                dataSpec = new BranchesForPublicSpec(
                    request.PageNumber,
                    request.PageSize,
                    request.search
                );
            }

            var items = await _unitOfWork.Branches.ListAsync(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Branches.CountAsync(dataSpec, cancellationToken);

            return new PaginatedResult<GetBranchesForPublicQueryResponse>(items, totalCount, request.PageNumber, request.PageSize);
        }


    }
}
