using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Domain.Entities
{
    public class EmployeeRolePermission : BaseEntity<int>
    {
        public EmployeeRolePermission(int employeeRoleId, Permission permission)
        {
            EmployeeRoleId = employeeRoleId;
            Permission = permission;
        }

        public int EmployeeRoleId { get; set; }
        public Permission Permission { get; set; }

        public EmployeeRole EmployeeRole { get; set; } = null!;
    }
}
