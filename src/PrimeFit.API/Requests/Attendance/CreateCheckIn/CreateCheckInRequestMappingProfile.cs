using AutoMapper;
using PrimeFit.Application.Features.Attendance.Commands.CreateCheckIn;

namespace PrimeFit.Api.Requests.Attendance.CreateCheckIn;

public class CreateCheckInRequestMappingProfile : Profile
{
    public CreateCheckInRequestMappingProfile()
    {
        CreateMap<CreateCheckInRequest, CreateCheckInCommand>();
    }
}
