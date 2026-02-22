using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Users.Queries.GetUsersPaginated
{
    public class GetMyBranchesQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public Governorate Governorate { get; set; } = null!;
        public string Address { get; private set; } = null!;
        public BranchType BranchType { get; set; }
        public BranchStatus BranchStatus { get; set; }
        public int SubscriptionsCount { get; set; }

    }

}
