using AutoMapper;
using PrimeFit.Application.Features.Users.Queries.CheckEmailAvailability;

namespace PrimeFit.Api.Requests.Users.CheckEmailAvailability;

public class CheckEmailAvailabilityRequestMappingProfile : Profile
{
    public CheckEmailAvailabilityRequestMappingProfile()
    {
        CreateMap<CheckEmailAvailabilityRequest, CheckEmailAvailabilityQuery>();
    }
}
