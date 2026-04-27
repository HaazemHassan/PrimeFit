using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Security
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class BranchAuthorizeAttribute : Attribute
    {
        public Permission[] BranchPermissions { get; set; } = [];
        public UserRole[] BranchRoles { get; set; } = [];
        public string? Policy { get; set; }
    }
}
