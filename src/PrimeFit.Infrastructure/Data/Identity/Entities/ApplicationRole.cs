using Microsoft.AspNetCore.Identity;

namespace PrimeFit.Infrastructure.Data.Identity.Entities
{
    public class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole()
        {

        }
        public ApplicationRole(string role)
        {
            Name = role;
        }

        public virtual ICollection<RolePermission> Permissions { get; set; } = new HashSet<RolePermission>();
    }
}

