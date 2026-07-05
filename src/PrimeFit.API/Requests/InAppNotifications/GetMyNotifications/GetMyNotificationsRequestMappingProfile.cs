using AutoMapper;
using PrimeFit.Application.Features.InAppNotifications.Queries.GetMyNotifications;

namespace PrimeFit.Api.Requests.InAppNotifications.GetMyNotifications;

public class GetMyNotificationsRequestMappingProfile : Profile
{
    public GetMyNotificationsRequestMappingProfile()
    {
        CreateMap<GetMyNotificationsRequest, GetMyNotificationsQuery>();
    }
}
