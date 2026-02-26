using PrimeFit.Application.Features.Branches.Queries.GetMyBranches;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchById
{
    public class GetBranchByIdQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ImageDto Logo { get; set; } = null!;
        public GovernorateDto Governorate { get; set; } = null!;
        public string Address { get; private set; } = null!;
        public BranchType BranchType { get; set; }
        public BranchStatus BranchStatus { get; set; }
        public bool IsOpenNow { get; set; }
        public bool ActivePackages { get; set; }
        public int ActiveSubscriptions { get; set; }


    }
}
