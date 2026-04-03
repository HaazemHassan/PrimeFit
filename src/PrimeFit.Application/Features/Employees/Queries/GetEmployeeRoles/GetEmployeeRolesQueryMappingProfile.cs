using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Employees.Queries.GetEmployeeRoles
{
    public class GetEmployeeRolesQueryMappingProfile : Profile
    {
        public GetEmployeeRolesQueryMappingProfile()
        {
            CreateMap<EmployeeRole, GetEmployeeRolesQueryResponse>();
        }
    }
}
