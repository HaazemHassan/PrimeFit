using AutoMapper;
using PrimeFit.Application.Features.Authentication.Commands.ChangePassword;

namespace PrimeFit.Api.Requests.Authentication.ChangePassword;

public class ChangePasswordRequestMappingProfile : Profile
{
    public ChangePasswordRequestMappingProfile()
    {
        CreateMap<ChangePasswordRequest, ChangePasswordCommand>();
    }
}
