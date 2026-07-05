using AutoMapper;
using PrimeFit.Application.Features.Authentication.Commands.SignInWithGoogle;

namespace PrimeFit.Api.Requests.Authentication.SignInWithGoogle;

public class SignInWithGoogleRequestMappingProfile : Profile
{
    public SignInWithGoogleRequestMappingProfile()
    {
        CreateMap<SignInWithGoogleRequest, SignInWithGoogleCommand>();
    }
}
