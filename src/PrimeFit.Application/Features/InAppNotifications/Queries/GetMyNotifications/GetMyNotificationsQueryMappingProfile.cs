using AutoMapper;
using PrimeFit.Application.Common.DTOS;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.InAppNotifications.Queries.GetMyNotifications
{
    public class GetMyNotificationsQueryMappingProfile : Profile
    {
        public GetMyNotificationsQueryMappingProfile()
        {
            CreateMap<UserNotification, InAppNotificationDto>();
        }
    }
}
