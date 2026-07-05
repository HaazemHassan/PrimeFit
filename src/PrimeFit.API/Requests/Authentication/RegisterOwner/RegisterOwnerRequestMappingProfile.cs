using AutoMapper;
using PrimeFit.Application.Features.Authentication.Commands.RegisterOwner;

namespace PrimeFit.Api.Requests.Authentication.RegisterOwner;

public class RegisterOwnerRequestMappingProfile : Profile
{
    public RegisterOwnerRequestMappingProfile()
    {
        CreateMap<RegisterOwnerRequest, RegisterOwnerCommand>();
    }
}
