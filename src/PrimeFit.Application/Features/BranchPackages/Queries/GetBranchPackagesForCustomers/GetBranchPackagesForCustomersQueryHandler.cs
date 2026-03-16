using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackagesForCustomers;
using PrimeFit.Application.Specifications.BranchPackages;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Packages.Queries.GetBranchPackagesForCustomers
{
    public class GetBranchPackagesForCustomersQueryHandler : IRequestHandler<GetBranchPackagesForCustomersQuery, ErrorOr<PaginatedResult<GetBranchPackagesForCustomersQueryResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetBranchPackagesForCustomersQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<PaginatedResult<GetBranchPackagesForCustomersQueryResponse>>> Handle(GetBranchPackagesForCustomersQuery request, CancellationToken cancellationToken)
        {

            var packagesSpec = new ActivePackagesPaginatedForBranchSpec(request.BranchId, request.PageNumber, request.PageSize);
            var packagesCountSpec = new BranchActivePackagesSpec(request.BranchId);


            var packages = await _unitOfWork.Packages.ListAsync<GetBranchPackagesForCustomersQueryResponse>(packagesSpec, cancellationToken);
            var totalCount = await _unitOfWork.Packages.CountAsync(packagesCountSpec, cancellationToken);


            return new PaginatedResult<GetBranchPackagesForCustomersQueryResponse>(packages, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
