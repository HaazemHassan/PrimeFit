using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Features.Packages.Queries.GetBranchPackagesForCustomers;

namespace PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackagesForCustomers
{
    public class GetBranchPackagesForCustomersQuery : PaginatedQuery
        , IRequest<ErrorOr<PaginatedResult<GetBranchPackagesForCustomersQueryResponse>>>
    {


        public GetBranchPackagesForCustomersQuery(int branchId, int pageNumber, int pageSize)
        {
            BranchId = branchId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public int BranchId { get; set; }


    }
}
