using AutoMapper;
using PrimeFit.Application.Features.Authentication.Commands.ResetPassword;

namespace PrimeFit.Api.Requests.Authentication.ResetPassword;

public class ResetPasswordRequestMappingProfile : Profile
{
    public ResetPasswordRequestMappingProfile()
    {
        CreateMap<ResetPasswordRequest, ResetPasswordCommand>();
    }
}
