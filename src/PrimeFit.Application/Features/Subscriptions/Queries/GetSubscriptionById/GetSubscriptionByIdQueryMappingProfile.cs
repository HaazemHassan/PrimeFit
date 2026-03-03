using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetSubscriptionById
{
    public class GetSubscriptionByIdQueryMappingProfile : Profile
    {

        public GetSubscriptionByIdQueryMappingProfile()
        {


            CreateMap<DomainUser, MemberDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));


            CreateMap<Subscription, GetSubscriptionByIdQueryResponse>()
               .ForMember(dest => dest.SubscriptionId, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
               .ForMember(dest => dest.ActivationDate, opt => opt.MapFrom(src => src.ActivationDate))
               .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.EndDate))
               .ForMember(dest => dest.DurationInMonths, opt => opt.MapFrom(src => src.DurationInMonths))
               .ForMember(dest => dest.PaidAmount, opt => opt.MapFrom(src => src.PaidAmount))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
               .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Package.Name))
               .ForMember(dest => dest.TotalFreezesCount, opt => opt.MapFrom(src => src.AllowedFreezeCount))
               .ForMember(dest => dest.RemainingFreezesCount, opt => opt.MapFrom(src => src.AllowedFreezeCount - src.Freezes.Count()))
               .ForMember(dest => dest.Member, opt => opt.MapFrom(src => src.User));

        }
    }
}
