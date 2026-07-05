using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Api.Requests.Branches.UpdateBasicDetails {
    public class UpdateBasicDetailsRequest
    {
        public string? Name { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public BranchType? BranchType { get; set; }
    }
}
