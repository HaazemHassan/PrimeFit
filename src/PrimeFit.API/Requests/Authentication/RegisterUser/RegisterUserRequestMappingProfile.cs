using AutoMapper;
using PrimeFit.Application.Features.Authentication.Commands.RegisterUser;

namespace PrimeFit.Api.Requests.Authentication.RegisterUser;

public class RegisterUserRequestMappingProfile : Profile
{
    public RegisterUserRequestMappingProfile()
    {
        CreateMap<RegisterUserRequest, RegisterUserCommand>();
    }
}
