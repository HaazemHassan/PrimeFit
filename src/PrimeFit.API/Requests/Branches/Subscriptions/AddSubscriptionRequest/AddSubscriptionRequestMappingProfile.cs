using AutoMapper;
using PrimeFit.Application.Features.Subscriptions.Commands.AddSubscription;

namespace PrimeFit.API.Requests.Branches.Subscriptions
{
    public class AddSubscriptionRequestMappingProfile : Profile
    {
        public AddSubscriptionRequestMappingProfile()
        {
            CreateMap<AddSubscriptionRequest, AddSubscriptionCommand>();
        }
    }
}
