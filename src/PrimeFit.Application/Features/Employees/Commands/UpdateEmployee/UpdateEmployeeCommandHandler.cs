using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Application.Specifications.Employees;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, ErrorOr<UpdateEmployeeCommandResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<EmployeeRole> _employeeRoleRepository;
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IBranchAuthorizationService _branchAuthorizationService;
        private readonly IMapper _mapper;

        public UpdateEmployeeCommandHandler(
            IUnitOfWork unitOfWork,
            IGenericRepository<EmployeeRole> employeeRoleRepository,
            IPhoneNumberService phoneNumberService,
            IApplicationUserService applicationUserService,
            IBranchAuthorizationService branchAuthorizationService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _employeeRoleRepository = employeeRoleRepository;
            _phoneNumberService = phoneNumberService;
            _applicationUserService = applicationUserService;
            _branchAuthorizationService = branchAuthorizationService;
            _mapper = mapper;
        }

        public async Task<ErrorOr<UpdateEmployeeCommandResponse>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var authResult = await _branchAuthorizationService.AuthorizeAsync(
                                        request.BranchId,
                                        Permission.EmployeesWrite,
                                        cancellationToken
                                    );

            if (authResult.IsError)
            {
                return authResult.Errors;
            }

            var employeeSpec = new EmployeeByIdAndBranchIdSpec(request.EmployeeId, request.BranchId);
            var employee = await _unitOfWork.Employees.FirstOrDefaultAsync(employeeSpec, cancellationToken);

            if (employee is null)
            {
                return Error.NotFound(
                    code: ErrorCodes.Employee.NotFound,
                    description: "Employee not found.");
            }

            if (request.Active.HasValue)
            {
                employee.UpdateStatus(request.Active.Value);
            }


            if (request.EmployeeRoleId.HasValue)
            {
                var employeeRole = await _employeeRoleRepository.GetAsync(r => r.Id == request.EmployeeRoleId.Value, cancellationToken);
                if (employeeRole is null)
                {
                    return Error.NotFound(
                        code: ErrorCodes.Employee.RoleNotFound,
                        description: "Employee role not found.");
                }

                employee.UpdateRole(request.EmployeeRoleId.Value);
            }


            string? normalizedPhone = null;

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                normalizedPhone = _phoneNumberService.Normalize(request.PhoneNumber!);

                var isPhoneExists = await _unitOfWork.Users.AnyAsync(
                    u => u.PhoneNumber == normalizedPhone && u.Id != employee.UserId,
                    cancellationToken);

                if (isPhoneExists)
                {
                    return Error.Conflict(
                        code: ErrorCodes.User.PhoneAlreadyExists,
                        description: "Phone number already exists");
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var isEmailExists = await _unitOfWork.Users.AnyAsync(
                    u => u.Email == request.Email && u.Id != employee.UserId,
                    cancellationToken);

                if (isEmailExists)
                {
                    return Error.Conflict(
                        code: ErrorCodes.User.EmailAlreadyExists,
                        description: "Email already exists");
                }

                employee.User.UpdateEmail(request.Email);

            }

            employee.User.UpdateInfo(request.FirstName, request.LastName, normalizedPhone);


            var appUserUpdateResult = await _applicationUserService.UpdateLinkedUser(
                employee.UserId,
                request.Email,
                normalizedPhone,
                cancellationToken);

            if (appUserUpdateResult.IsError)
            {
                return appUserUpdateResult.Errors;
            }

            var response = _mapper.Map<UpdateEmployeeCommandResponse>(employee);
            return response;
        }
    }
}
