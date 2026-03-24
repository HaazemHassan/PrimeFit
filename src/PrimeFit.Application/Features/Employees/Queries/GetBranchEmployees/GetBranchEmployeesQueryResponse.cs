namespace PrimeFit.Application.Features.Employees.Queries.GetBranchEmployees
{
    public class GetBranchEmployeesQueryResponse
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
