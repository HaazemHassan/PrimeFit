using AutoMapper;
using PrimeFit.Application.Features.Subscriptions.Commands.AddSubscription;

namespace PrimeFit.Api.Requests.Subscriptions.AddSubscription {
    public class AddSubscriptionRequestMappingProfile : Profile
    {
        public AddSubscriptionRequestMappingProfile()
        {
            CreateMap<AddSubscriptionRequest, AddSubscriptionCommand>();
        }
    }
}
