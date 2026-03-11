using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.Specifications.BranchPackages;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackages
{
    public class GetBranchPackagesQueryHandler : IRequestHandler<GetBranchPackagesQuery, ErrorOr<PaginatedResult<GetBranchPackagesQueryResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBranchAuthorizationService _branchAuthorizationService;

        public GetBranchPackagesQueryHandler(IUnitOfWork unitOfWork, IBranchAuthorizationService branchAuthorizationService)
        {
            _unitOfWork = unitOfWork;
            _branchAuthorizationService = branchAuthorizationService;
        }

        public async Task<ErrorOr<PaginatedResult<GetBranchPackagesQueryResponse>>> Handle(GetBranchPackagesQuery request, CancellationToken cancellationToken)
        {
            var authResult = await _branchAuthorizationService.AuthorizeAsync(request.BranchId, Permission.PackagesView, cancellationToken);
            if (authResult.IsError)
            {
                return authResult.Errors;
            }

            var branchPackagesPaginatedSpec = new BranchPackagesPaginatedSpec(
                request.BranchId,
                request.PageNumber,
                request.PageSize);

            var branchPackagesSpec = new BranchPackagesSpec(request.BranchId);
            var activePackagesSpec = new BranchActivePackagesSpec(request.BranchId);


            var packages = await _unitOfWork.Packages.
                ListAsync<GetBranchPackagesQueryResponse>(branchPackagesPaginatedSpec, cancellationToken);

            int totalCount = 0, activeCount = 0;
            decimal averagePrice = 0;

            if (packages.Count != 0)
            {
                totalCount = await _unitOfWork.Packages.CountAsync(branchPackagesSpec, cancellationToken);
                activeCount = await _unitOfWork.Packages.CountAsync(activePackagesSpec, cancellationToken);
                averagePrice = await _unitOfWork.Packages.AverageAsync(p => p.Price, branchPackagesSpec, ct: cancellationToken);
            }



            var result = new PaginatedResult<GetBranchPackagesQueryResponse>(packages, totalCount, request.PageNumber, request.PageSize)
            {
                Meta = new PackagesStatsMeta(totalCount, activeCount, averagePrice)
            };

            return result;
        }
    }
}
