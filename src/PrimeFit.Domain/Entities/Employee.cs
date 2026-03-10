using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Domain.Entities
{
    public class Employee : FullAuditableEntity<int>
    {
        public Employee(int userId, int branchId, int roleId)
        {
            UserId = userId;
            BranchId = branchId;
            RoleId = roleId;
        }

        public int UserId { get; private set; }
        public int BranchId { get; private set; }
        public int RoleId { get; private set; }

        public DomainUser User { get; private set; } = null!;
        public Branch Branch { get; private set; } = null!;
        public EmployeeRole Role { get; private set; } = null!;
    }
}
