using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Infrastructure.Data.Identity.Entities
{
    public class RolePermission
    {
        public int RoleId { get; set; }
        public Permission Permission { get; set; }

        public virtual ApplicationRole Role { get; set; } = null!;
    }
}
