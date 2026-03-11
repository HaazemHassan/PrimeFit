using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackages
{
    public class GetBranchPackagesQueryMappingProfile : Profile
    {
        public GetBranchPackagesQueryMappingProfile()
        {
            CreateMap<Package, GetBranchPackagesQueryResponse>();

        }
    }
}
