using ErrorOr;
using MediatR;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Employees.Queries.GetEmployeeRoles
{
    public class GetEmployeeRolesQueryHandler : IRequestHandler<GetEmployeeRolesQuery, ErrorOr<List<GetEmployeeRolesQueryResponse>>>
    {
        private readonly IGenericRepository<EmployeeRole> _employeeRoleRepository;

        public GetEmployeeRolesQueryHandler(IGenericRepository<EmployeeRole> employeeRoleRepository)
        {
            _employeeRoleRepository = employeeRoleRepository;
        }

        public async Task<ErrorOr<List<GetEmployeeRolesQueryResponse>>> Handle(GetEmployeeRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _employeeRoleRepository.ListAsync<GetEmployeeRolesQueryResponse>(role => true, cancellationToken);
            return roles;
        }
    }
}
