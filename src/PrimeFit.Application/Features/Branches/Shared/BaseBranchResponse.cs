using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Branches.Shared
{
    public class BaseBranchResponse
    {
        public string Name { get; set; } = null!;
        public Governorate Governorate { get; set; } = null!;
        public string Address { get; private set; } = null!;
        public BranchType BranchType { get; set; }
        public string BranchRate { get; set; } = null!;

    }
}
