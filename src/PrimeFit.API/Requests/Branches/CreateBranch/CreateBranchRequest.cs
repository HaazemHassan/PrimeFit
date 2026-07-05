using PrimeFit.Domain.Common.Enums;
using ErrorOr;

namespace PrimeFit.Api.Requests.Branches.CreateBranch;

public class CreateBranchRequest
{
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public BranchType BranchType { get; set; }
}
