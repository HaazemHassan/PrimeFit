using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Members.Commands.CreateMemberWithSubscription
{
    public class CreateMemberWithSubscriptionCommandMappingProfile : Profile
    {
        public CreateMemberWithSubscriptionCommandMappingProfile()
        {
            CreateMap<Subscription, CreateMemberWithSubscriptionCommandResponse>();
        }
    }
}

