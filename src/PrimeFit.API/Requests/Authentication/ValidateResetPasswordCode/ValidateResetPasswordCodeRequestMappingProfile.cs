using AutoMapper;
using PrimeFit.Application.Features.Authentication.Queries.ValidateResetPasswordCode;

namespace PrimeFit.Api.Requests.Authentication.ValidateResetPasswordCode;

public class ValidateResetPasswordCodeRequestMappingProfile : Profile
{
    public ValidateResetPasswordCodeRequestMappingProfile()
    {
        CreateMap<ValidateResetPasswordCodeRequest, ValidateResetPasswordCodeQuery>();
    }
}
