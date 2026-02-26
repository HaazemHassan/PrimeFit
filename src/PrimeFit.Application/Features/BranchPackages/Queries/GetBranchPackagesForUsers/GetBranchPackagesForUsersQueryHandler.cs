using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Specifications.BranchPackages;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Packages.Queries.GetBranchPackagesForUsers
{
    public class GetBranchPackagesForUsersQueryHandler : IRequestHandler<GetBranchPackagesForUsersQuery, ErrorOr<PaginatedResult<GetBranchPackagesForUsersQueryResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetBranchPackagesForUsersQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<PaginatedResult<GetBranchPackagesForUsersQueryResponse>>> Handle(GetBranchPackagesForUsersQuery request, CancellationToken cancellationToken)
        {

            var packagesSpec = new ActivePackagesPaginatedForBranchSpec(request.BranchId, request.PageNumber, request.PageSize);
            var packagesCountSpec = new BranchActivePackagesSpec(request.BranchId);


            var packages = await _unitOfWork.Packages.ListAsync<GetBranchPackagesForUsersQueryResponse>(packagesSpec, cancellationToken);
            var totalCount = await _unitOfWork.Packages.CountAsync(packagesCountSpec, cancellationToken);


            return new PaginatedResult<GetBranchPackagesForUsersQueryResponse>(packages, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
