using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackagesForOwner
{
    public class GetBranchPackagesForOwnerQueryMappingProfile : Profile
    {
        public GetBranchPackagesForOwnerQueryMappingProfile()
        {
            CreateMap<Package, GetBranchPackagesForOwnerQueryResponse>();

        }
    }
}
