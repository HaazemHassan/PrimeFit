using PrimeFit.Domain.Primitives.PrimeFit.Domain.Primitives;

namespace PrimeFit.Domain.Entities
{
    public class EmployeeRole : BaseEntity<int>
    {
        public EmployeeRole(string name, string? description = null)
        {
            Name = name;
            Description = description;
            Permissions = new HashSet<EmployeeRolePermission>();
            Employees = new HashSet<Employee>();
        }

        public string Name { get; private set; }
        public string? Description { get; private set; }

        public ICollection<EmployeeRolePermission> Permissions { get; private set; }
        public ICollection<Employee> Employees { get; private set; }
    }
}
