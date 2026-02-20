using ErrorOr;
using Microsoft.AspNetCore.Identity;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;
using PrimeFit.Infrastructure.Data.Identity;
using PrimeFit.Infrastructure.Data.Identity.Entities;

namespace PrimeFit.Infrastructure.Services
{
    internal class ApplicationUserService : IApplicationUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientContextService _clientContextService;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;


        public ApplicationUserService(IUnitOfWork unitOfWork, IClientContextService clientContextService, ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _clientContextService = clientContextService;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<ErrorOr<DomainUser>> AddUser(DomainUser user, string password, UserRole role = UserRole.User, CancellationToken ct = default)
        {
            var userByPhone = await _unitOfWork.Users.AnyAsync(u => u.PhoneNumber == user.PhoneNumber, ct);

            if (userByPhone)
                return Error.Conflict(code: ErrorCodes.User.PhoneAlreadyExists, description: "Phone number already exists");

            var applicationUser = new ApplicationUser(user.Email, user.PhoneNumber);
            applicationUser.AssignDomainUser(user);

            var createResult = await _userManager.CreateAsync(applicationUser, password);

            if (!createResult.Succeeded)
            {
                if (createResult.HasError(IdentityErrorCodes.DuplicateEmail))

                    return Error.Conflict(code: ErrorCodes.User.EmailAlreadyExists, description: "This Email already exists");

                return Error.Failure(description: "Failed to create user");

            }


            var addToRoleResult = await _userManager.AddToRoleAsync(applicationUser, role.ToString()!);
            if (!addToRoleResult.Succeeded)
                return Error.Failure(description: "Failed to create user");


            return user;

        }

    }
}
