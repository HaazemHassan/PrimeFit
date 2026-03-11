using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Features.Packages.Queries.GetBranchPackagesForCustomers;

namespace PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackagesForCustomers
{
    public class GetBranchPackagesForCustomersQuery : PaginatedQuery
        , IRequest<ErrorOr<PaginatedResult<GetBranchPackagesForCustomersQueryResponse>>>
    {


        public GetBranchPackagesForCustomersQuery(int branchId)
        {
            BranchId = branchId;
        }
        public int BranchId { get; set; }


    }
}
