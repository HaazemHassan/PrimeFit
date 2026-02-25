using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.API.Requests.Owner.Branches
{
    public class UpdateBussinessDetailsRequest
    {
        public string? Name { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public BranchType? BranchType { get; set; }
    }
}
