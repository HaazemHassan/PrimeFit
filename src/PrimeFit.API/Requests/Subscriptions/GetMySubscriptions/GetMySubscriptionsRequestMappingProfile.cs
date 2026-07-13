using AutoMapper;
using PrimeFit.Application.Features.Subscriptions.Queries.GetMySubscriptions;

namespace PrimeFit.Api.Requests.Subscriptions.GetMySubscriptions
{
    public class GetMySubscriptionsRequestMappingProfile : Profile
    {
        public GetMySubscriptionsRequestMappingProfile()
        {
            CreateMap<GetMySubscriptionsRequest, GetMySubscriptionsQuery>();
        }
    }
}
