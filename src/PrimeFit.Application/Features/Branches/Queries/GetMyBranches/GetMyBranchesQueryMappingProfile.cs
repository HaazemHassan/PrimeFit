using AutoMapper;
using PrimeFit.Application.Features.Users.Queries.GetUsersPaginated;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Branches.Queries.GetMyBranches
{
    public class GetMyBranchesQueryMappingProfile : Profile
    {
        public GetMyBranchesQueryMappingProfile()
        {


            CreateMap<BranchImage, ImageDto>();
            CreateMap<Governorate, GovernorateDto>();

            CreateMap<Branch, GetMyBranchesQueryResponse>().
                ForMember(dest => dest.Logo, opt => opt.MapFrom(src => src.Images
                   .Where(i => i.Type == BranchImageType.Logo)
                   .FirstOrDefault()));



        }

    }
}


