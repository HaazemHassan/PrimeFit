using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Packages.Queries.GetBranchPackagesForOwner
{
    public class GetBranchPackagesForOwnerQueryMappingProfile : Profile
    {
        public GetBranchPackagesForOwnerQueryMappingProfile()
        {
            CreateMap<Package, GetBranchPackagesForOwnerQueryResponse>();
        }
    }
}
