namespace PrimeFit.Application.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommandResponse
    {
        public CreateEmployeeCommandResponse(int employeeId, int userId, string fullName, string email, string roleName)
        {
            EmployeeId = employeeId;
            UserId = userId;
            FullName = fullName;
            Email = email;
            RoleName = roleName;
        }

        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}
