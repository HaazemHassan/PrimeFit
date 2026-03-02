using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Branches.Queries.GetMyBranches
{
    public class GetMyBranchesQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public BranchType BranchType { get; set; }
        public BranchStatus BranchStatus { get; set; }
        public string LogoUrl { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}

