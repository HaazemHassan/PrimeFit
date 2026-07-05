using AutoMapper;
using PrimeFit.Application.Features.Authentication.Commands.RefreshToken;

namespace PrimeFit.Api.Requests.Authentication.RefreshToken;

public class RefreshTokenRequestMappingProfile : Profile
{
    public RefreshTokenRequestMappingProfile()
    {
        CreateMap<RefreshTokenRequest, RefreshTokenCommand>();
    }
}
