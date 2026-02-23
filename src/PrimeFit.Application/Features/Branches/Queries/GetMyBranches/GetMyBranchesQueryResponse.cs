using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Users.Queries.GetUsersPaginated
{
    public class GetMyBranchesQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public GovernorateDto Governorate { get; set; } = null!;
        public string Address { get; private set; } = null!;
        public BranchType BranchType { get; set; }
        public BranchStatus BranchStatus { get; set; }
        public int SubscriptionsCount { get; set; }

    }

    public class GovernorateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

    }


}
