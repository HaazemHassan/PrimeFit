using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Employees.Queries.GetBranchEmployees
{
    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    public class GetBranchEmployeesQuery : PaginatedQuery, IRequest<ErrorOr<PaginatedResult<GetBranchEmployeesQueryResponse>>>, IAuthorizedRequest
    {
        public int BranchId { get; set; }
    }
}
