using AutoMapper;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetMySubscriptionById
{
    public class GetMySubscriptionByIdMappingProfile : Profile
    {
        public GetMySubscriptionByIdMappingProfile()
        {
            CreateMap<Subscription, GetMySubscriptionByIdResponse>()
                .ForMember(dest => dest.SubscriptionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SubscriptionStatus, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Branch.Images))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Branch.Address))
                .ForMember(dest => dest.Governorate, opt => opt.MapFrom(src => src.Branch.Governorate != null ? src.Branch.Governorate.Name : null))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Branch.Coordinates != null ? (double?)src.Branch.Coordinates.Y : null))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Branch.Coordinates != null ? (double?)src.Branch.Coordinates.X : null))
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Package.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PaidAmount))
                .ForMember(dest => dest.ActivationDate, opt => opt.MapFrom(src => src.ActivationDate))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Status == SubscriptionStatus.Frozen ? null : src.EndDate))
                .ForMember(dest => dest.DurationInDays, opt => opt.MapFrom(src => src.DurationInMonths * 30))
                .ForMember(dest => dest.CheckInsCount, opt => opt.MapFrom(src => src.CheckIns.Count));
        }
    }
}