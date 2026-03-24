namespace PrimeFit.API.Requests.Employees.UpdateEmployeeRequest
{
    public class UpdateEmployeeRequest
    {
        public int BranchId { get; set; }
        public int? EmployeeRoleId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? Active { get; set; }
    }
}
