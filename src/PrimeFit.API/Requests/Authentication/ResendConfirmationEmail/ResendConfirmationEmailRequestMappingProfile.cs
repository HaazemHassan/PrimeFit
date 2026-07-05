using AutoMapper;
using PrimeFit.Application.Features.Authentication.Commands.ResendConfirmEmail;

namespace PrimeFit.Api.Requests.Authentication.ResendConfirmationEmail;

public class ResendConfirmationEmailRequestMappingProfile : Profile
{
    public ResendConfirmationEmailRequestMappingProfile()
    {
        CreateMap<ResendConfirmationEmailRequest, ResendConfirmationEmailCommand>();
    }
}
