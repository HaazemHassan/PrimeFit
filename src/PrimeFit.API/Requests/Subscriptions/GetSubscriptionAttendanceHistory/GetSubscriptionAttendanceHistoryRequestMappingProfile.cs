using AutoMapper;
using PrimeFit.Application.Features.Subscriptions.Queries.GetSubscriptionAttendanceHistory;

namespace PrimeFit.Api.Requests.Subscriptions.GetSubscriptionAttendanceHistory {
    public class GetSubscriptionAttendanceHistoryRequestMappingProfile : Profile
    {
        public GetSubscriptionAttendanceHistoryRequestMappingProfile()
        {
            CreateMap<GetSubscriptionAttendanceHistoryRequest, GetSubscriptionAttendanceHistoryQuery>();
        }
    }
}
