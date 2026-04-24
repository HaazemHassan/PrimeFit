using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Transaction;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Employees.Commands.UpdateEmployee
{
    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    public class UpdateEmployeeCommand : IRequest<ErrorOr<UpdateEmployeeCommandResponse>>, IAuthorizedRequest, ITransactionalRequest
    {
        public int EmployeeId { get; set; }
        public int BranchId { get; set; }
        public int? EmployeeRoleId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? Active { get; set; }
    }
}
