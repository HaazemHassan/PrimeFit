using AutoMapper;
using PrimeFit.Application.Features.Employees.Commands.UpdateEmployee;

namespace PrimeFit.Api.Requests.Employees.UpdateEmployeeRequest {
    public class UpdateEmployeeRequestMappingProfile : Profile
    {
        public UpdateEmployeeRequestMappingProfile()
        {
            CreateMap<UpdateEmployeeRequest, UpdateEmployeeCommand>();
        }
    }
}
