using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Specifications.Packages;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Packages.Queries.GetBranchPackagesForOwner
{
    public class GetBranchPackagesForOwnerQueryHandler : IRequestHandler<GetBranchPackagesForOwnerQuery, ErrorOr<PaginatedResult<GetBranchPackagesForOwnerQueryResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetBranchPackagesForOwnerQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<PaginatedResult<GetBranchPackagesForOwnerQueryResponse>>> Handle(GetBranchPackagesForOwnerQuery request, CancellationToken cancellationToken)
        {
            var ownerId = _currentUserService.UserId!.Value;


            var packagesForOwnerSpec = new PackagesPaginatedForOwnerSpec(
                request.BranchId,
                ownerId,
                request.PageNumber,
                request.PageSize);


            var packagesCountForOwnerSpec = new PackagesCountForOwnerSpec(
                request.BranchId,
                ownerId);

            var packages = await _unitOfWork.Packages.ListAsync<GetBranchPackagesForOwnerQueryResponse>(packagesForOwnerSpec, cancellationToken);
            var totalCount = await _unitOfWork.Packages.CountAsync(packagesCountForOwnerSpec, cancellationToken);

            return new PaginatedResult<GetBranchPackagesForOwnerQueryResponse>(packages, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
