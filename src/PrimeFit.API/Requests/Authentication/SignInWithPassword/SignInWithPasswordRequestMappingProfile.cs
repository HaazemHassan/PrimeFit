using AutoMapper;
using PrimeFit.Application.Features.Authentication.Commands.SignIn;

namespace PrimeFit.Api.Requests.Authentication.SignInWithPassword;

public class SignInWithPasswordRequestMappingProfile : Profile
{
    public SignInWithPasswordRequestMappingProfile()
    {
        CreateMap<SignInWithPasswordRequest, SignInWithPasswordCommand>();
    }
}
