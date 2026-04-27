using PrimeFit.Domain.Common.Enums;
﻿using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Transaction;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Application.Security.Policies;

namespace PrimeFit.Application.Features.Employees.Commands.CreateEmployee
{
    [Authorize(Policy = AuthorizationPolicies.BranchStaffOnly)]
    [BranchAuthorize(BranchPermissions = [Permission.EmployeesWrite])]
    public class CreateEmployeeCommand : IRequest<ErrorOr<CreateEmployeeCommandResponse>>, IBranchAuthorizedRequest, ITransactionalRequest
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
