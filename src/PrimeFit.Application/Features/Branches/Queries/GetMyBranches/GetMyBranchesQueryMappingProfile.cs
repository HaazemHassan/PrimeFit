using AutoMapper;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Branches.Queries.GetMyBranches
{
    public class GetMyBranchesQueryMappingProfile : Profile
    {
        public GetMyBranchesQueryMappingProfile()
        {

            CreateMap<Branch, GetMyBranchesQueryResponse>()
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.Images
                   .Where(i => i.Type == BranchImageType.Logo && i.Status == BranchImageStatus.Active).Select(i => i.Url)
                   .FirstOrDefault()));

        }
    }
}



