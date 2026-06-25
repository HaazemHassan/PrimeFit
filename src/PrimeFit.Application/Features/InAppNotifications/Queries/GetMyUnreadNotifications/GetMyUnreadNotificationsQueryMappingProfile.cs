using AutoMapper;
using PrimeFit.Application.Common.DTOS;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.InAppNotifications.Queries.GetMyUnreadNotifications
{
    public class GetMyUnreadNotificationsQueryMappingProfile : Profile
    {
        public GetMyUnreadNotificationsQueryMappingProfile()
        {
            CreateMap<UserNotification, InAppNotificationDto>();
        }
    }
}
