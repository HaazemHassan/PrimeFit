using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Subscriptions.Commands.AddSubscription
{
    public class AddSubscriptionCommandMappingProfile : Profile
    {
        public AddSubscriptionCommandMappingProfile()
        {
            CreateMap<Subscription, AddSubscriptionCommandResponse>();
        }
    }
}

