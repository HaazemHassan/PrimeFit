using ErrorOr;
using MediatR;

namespace PrimeFit.Application.Features.Employees.Queries.GetEmployeeRoles
{
    public class GetEmployeeRolesQuery : IRequest<ErrorOr<List<GetEmployeeRolesQueryResponse>>>
    {
    }
}
