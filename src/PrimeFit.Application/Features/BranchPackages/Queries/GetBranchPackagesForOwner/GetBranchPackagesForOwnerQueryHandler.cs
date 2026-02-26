using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Specifications.BranchPackages;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackagesForOwner
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


            var OwnerBranchPackagesPaginatedSpec = new OwnerBranchPackagesPaginatedSpec(
                request.BranchId,
                ownerId,
                request.PageNumber,
                request.PageSize);

            var OwnerBranchPackagesSpec = new OwnerBranchPackageSpec(request.BranchId, ownerId);
            var ActivePackagesSpec = new BranchActivePackagesSpec(request.BranchId);


            var packages = await _unitOfWork.Packages.
                ListAsync<GetBranchPackagesForOwnerQueryResponse>(OwnerBranchPackagesPaginatedSpec, cancellationToken);

            int totalCount = 0, activeCount = 0;
            decimal averagePrice = 0;

            if (packages.Count != 0)
            {
                totalCount = await _unitOfWork.Packages.CountAsync(OwnerBranchPackagesSpec, cancellationToken);
                activeCount = await _unitOfWork.Packages.CountAsync(ActivePackagesSpec, cancellationToken);
                averagePrice = await _unitOfWork.Packages.AverageAsync(p => p.Price, OwnerBranchPackagesSpec, ct: cancellationToken);
            }



            var result = new PaginatedResult<GetBranchPackagesForOwnerQueryResponse>(packages, totalCount, request.PageNumber, request.PageSize);
            result.Meta = new PackagesStatsMeta(totalCount, activeCount, averagePrice);

            return result;
        }
    }
}
