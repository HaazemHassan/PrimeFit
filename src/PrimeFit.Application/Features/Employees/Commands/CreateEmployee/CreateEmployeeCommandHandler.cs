using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommandHandler(
        IUnitOfWork unitOfWork,
        IGenericRepository<EmployeeRole> employeeRoleRepository,
        IApplicationUserService applicationUserService,
        IPhoneNumberService phoneNumberService,
        ICurrentUserService currentUserService,
        IBranchAuthorizationService _branchAuthorizationService)
        : IRequestHandler<CreateEmployeeCommand, ErrorOr<CreateEmployeeCommandResponse>>
    {
        public async Task<ErrorOr<CreateEmployeeCommandResponse>> Handle(
            CreateEmployeeCommand request,
            CancellationToken cancellationToken)
        {

            var authResult = await _branchAuthorizationService.AuthorizeAsync(
                request.BranchId,
                Permission.EmployeesWrite,
                cancellationToken);

            if (authResult.IsError)
            {
                return authResult.Errors;
            }


            var branch = await unitOfWork.Branches.GetByIdAsync(request.BranchId, cancellationToken);
            if (branch is null)
            {
                return Error.NotFound(
                    code: ErrorCodes.Branch.BranchNotFound,
                    description: "Branch not found");

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
                return addUserResult.Errors;

            var createdUser = addUserResult.Value;

            var alreadyEmployee = await unitOfWork.Employees.AnyAsync(
                e => e.UserId == createdUser.Id && e.BranchId == request.BranchId,
                cancellationToken);

            if (alreadyEmployee)
                return Error.Conflict(
                    code: ErrorCodes.Employee.AlreadyExistsInBranch,
                    description: "This user is already an employee in this branch.");

            var employee = new Employee(createdUser.Id, request.BranchId, request.EmployeeRoleId);
            await unitOfWork.Employees.AddAsync(employee, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateEmployeeCommandResponse(
                employee.Id,
                createdUser.Id,
                $"{createdUser.FirstName} {createdUser.LastName}",
                createdUser.Email,
                employeeRole.Name);
        }
    }
}

