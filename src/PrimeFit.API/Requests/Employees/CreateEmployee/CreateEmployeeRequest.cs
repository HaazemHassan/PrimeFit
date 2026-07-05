using ErrorOr;

namespace PrimeFit.Api.Requests.Employees.CreateEmployee;

public class CreateEmployeeRequest
{
        public int BranchId { get; set; }
        public int EmployeeRoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
}
