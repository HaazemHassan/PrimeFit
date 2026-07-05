using AutoMapper;
using PrimeFit.Application.Features.Employees.Commands.CreateEmployee;

namespace PrimeFit.Api.Requests.Employees.CreateEmployee;

public class CreateEmployeeRequestMappingProfile : Profile
{
    public CreateEmployeeRequestMappingProfile()
    {
        CreateMap<CreateEmployeeRequest, CreateEmployeeCommand>();
    }
}
