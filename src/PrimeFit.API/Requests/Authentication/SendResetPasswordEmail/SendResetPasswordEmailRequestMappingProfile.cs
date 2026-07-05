using AutoMapper;
using PrimeFit.Application.Features.Authentication.Commands.SendResetPasswordEmail;

namespace PrimeFit.Api.Requests.Authentication.SendResetPasswordEmail;

public class SendResetPasswordEmailRequestMappingProfile : Profile
{
    public SendResetPasswordEmailRequestMappingProfile()
    {
        CreateMap<SendResetPasswordEmailRequest, SendResetPasswordEmailCommand>();
    }
}
