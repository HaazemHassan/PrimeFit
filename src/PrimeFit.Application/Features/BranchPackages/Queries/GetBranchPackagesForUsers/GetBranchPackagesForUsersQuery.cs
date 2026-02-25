using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;

namespace PrimeFit.Application.Features.Packages.Queries.GetBranchPackagesForUsers
{
    public class GetBranchPackagesForUsersQuery : PaginatedQuery
        , IRequest<ErrorOr<PaginatedResult<GetBranchPackagesForUsersQueryResponse>>>
    {


        public GetBranchPackagesForUsersQuery(int branchId)
        {
            BranchId = branchId;
        }
        public int BranchId { get; set; }


    }
}
