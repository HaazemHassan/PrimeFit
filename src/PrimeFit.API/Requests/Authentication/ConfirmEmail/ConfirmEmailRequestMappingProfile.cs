using AutoMapper;
using PrimeFit.Application.Features.Authentication.Commands.ConfirmEmail;

namespace PrimeFit.Api.Requests.Authentication.ConfirmEmail;

public class ConfirmEmailRequestMappingProfile : Profile
{
    public ConfirmEmailRequestMappingProfile()
    {
        CreateMap<ConfirmEmailRequest, ConfirmEmailCommand>();
    }
}
