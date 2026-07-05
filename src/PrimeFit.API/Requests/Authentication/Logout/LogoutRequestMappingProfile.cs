using AutoMapper;
using PrimeFit.Application.Features.Authentication.Commands.Logout;

namespace PrimeFit.Api.Requests.Authentication.Logout;

public class LogoutRequestMappingProfile : Profile
{
    public LogoutRequestMappingProfile()
    {
        CreateMap<LogoutRequest, LogoutCommand>();
    }
}
