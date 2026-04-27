using PrimeFit.Domain.Common.Enums;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Pagination;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Employees.Queries.GetBranchEmployees
{
    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    [BranchAuthorize(BranchPermissions = [Permission.EmployeesView])]
    public class GetBranchEmployeesQuery : PaginatedQuery, IRequest<ErrorOr<PaginatedResult<GetBranchEmployeesQueryResponse>>>, IBranchAuthorizedRequest
    {
        public int BranchId { get; set; }
    }
}
