using ErrorOr;
using MediatR;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommandHandler(
        IUnitOfWork unitOfWork,
        IGenericRepository<EmployeeRole> employeeRoleRepository,
        IApplicationUserService applicationUserService,
        IPhoneNumberService phoneNumberService,
        IBranchAuthorizationService _branchAuthorizationService)
        : IRequestHandler<CreateEmployeeCommand, ErrorOr<CreateEmployeeCommandResponse>>
    {
        public async Task<ErrorOr<CreateEmployeeCommandResponse>> Handle(
            CreateEmployeeCommand request,
            CancellationToken cancellationToken)
        {

            var branch = await unitOfWork.Branches.GetByIdAsync(request.BranchId, cancellationToken);
            if (branch is null)
            {
                return Error.NotFound(
                    code: ErrorCodes.Branch.BranchNotFound,
                    description: "Branch not found");

            }

            var authResult = await _branchAuthorizationService.AuthorizeAsync(
                request.BranchId,
                Permission.EmployeesWrite,
                cancellationToken);

            if (authResult.IsError)
            {
                return authResult.Errors;
            }

            var employeeRole = await employeeRoleRepository.GetAsync(
                        r => r.Id == request.EmployeeRoleId,
                        cancellationToken);

            if (employeeRole is null)
            {
                return Error.NotFound(
                    code: ErrorCodes.Employee.RoleNotFound,
                    description: "Employee role not found.");
            }


            var normalizedPhone = phoneNumberService.Normalize(request.PhoneNumber);
            var domainUser = new DomainUser(UserType.Employee, request.FirstName, request.LastName, request.Email, normalizedPhone);

            var addUserResult = await applicationUserService.AddUser(
                domainUser,
                request.Password,
                null,
                cancellationToken);

            if (addUserResult.IsError)
            {
                return addUserResult.Errors;
            }

            var employee = new Employee(domainUser.Id, request.BranchId, request.EmployeeRoleId);
            employee.UpdateStatus(active: true);

            await unitOfWork.Employees.AddAsync(employee, cancellationToken);

            return new CreateEmployeeCommandResponse(
                employee.Id,
                domainUser.Id,
                $"{domainUser.FirstName} {domainUser.LastName}",
                domainUser.Email,
                employeeRole.Name);
        }
    }
}

