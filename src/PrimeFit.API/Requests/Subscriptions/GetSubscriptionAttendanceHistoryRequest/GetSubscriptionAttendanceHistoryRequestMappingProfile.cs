using AutoMapper;
using PrimeFit.Application.Features.Subscriptions.Queries.GetSubscriptionAttendanceHistory;

namespace PrimeFit.API.Requests.Subscriptions
{
    public class GetSubscriptionAttendanceHistoryRequestMappingProfile : Profile
    {
        public GetSubscriptionAttendanceHistoryRequestMappingProfile()
        {
            CreateMap<GetSubscriptionAttendanceHistoryRequest, GetSubscriptionAttendanceHistoryQuery>();
        }
    }
}
