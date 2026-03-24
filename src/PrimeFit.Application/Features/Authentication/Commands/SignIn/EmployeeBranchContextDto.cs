namespace PrimeFit.Application.Features.Authentication.Commands.SignIn
{
    public class EmployeeBranchContextDto
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
