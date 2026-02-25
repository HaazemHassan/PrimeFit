using AutoMapper;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchById
{
    public class GetBranchByIdQueryMappingProfile : Profile
    {
        public GetBranchByIdQueryMappingProfile()
        {
            CreateMap<Branch, GetBranchByIdQueryResponse>()
                .ForMember(dest => dest.Logo, opt => opt.MapFrom(src => src.Images
                   .Where(i => i.Type == BranchImageType.Logo)
                   .FirstOrDefault()))
                .ForMember(dest => dest.ActivePackages, opt => opt.MapFrom(src => src.Packages.Count(p => p.IsActive)))
                .ForMember(dest => dest.ActiveSubscriptions, opt =>
                opt.MapFrom(src => src.Subscriptions.
                Count(s => s.Status == SubscriptionStatus.Active && s.EndDate > DateTime.UtcNow)));


        }
    }
}
