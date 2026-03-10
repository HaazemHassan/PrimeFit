using ErrorOr;
using MediatR;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;

namespace PrimeFit.Application.Features.Employees.Commands.CreateEmployee
{
    [Authorize]
    public class CreateEmployeeCommand : IRequest<ErrorOr<CreateEmployeeCommandResponse>>, IAuthorizedRequest
    {
        public int BranchId { get; set; }
        public int EmployeeRoleId { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
