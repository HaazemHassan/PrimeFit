using PrimeFit.Application.Features.Branches.Shared.DTOS;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchById
{
    public class GetBranchByIdQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public BranchType BranchType { get; set; }
        public BranchStatus BranchStatus { get; set; }
        public List<ImageDto> Images { get; set; } = null!;
        public GovernorateDto? Governorate { get; set; } = null!;
        public string Address { get; set; } = null!;
        public bool IsOpenNow { get; set; }
        public int ActivePackagesCount { get; set; }
        public int ActiveSubscriptionsCount { get; set; }


    }
}
