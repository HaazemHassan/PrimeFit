using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.RepositoriesContracts;
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

        public async Task<ErrorOr<int>> AddUser(
             DomainUser domainUser,
             string password,
             UserRole? userRole = default,
             CancellationToken ct = default)
        {
            var isAuthUserLinked = await _userManager.Users.AnyAsync(au => au.DomainUserId == domainUser.Id || au.Email == domainUser.Email, ct);

            if (isAuthUserLinked)
            {
                return Error.Conflict(
                    code: ErrorCodes.User.EmailAlreadyExists,
                    description: "Email already exists");
            }


            if (!string.IsNullOrWhiteSpace(domainUser.PhoneNumber))
            {
                var phoneExists = await _userManager.Users.AnyAsync(au => au.PhoneNumber == domainUser.PhoneNumber, ct);
                if (phoneExists)
                {
                    return Error.Conflict(
                        code: ErrorCodes.User.PhoneAlreadyExists,
                        description: "Phone number already exists");
                }
            }

            await _unitOfWork.Users.AddAsync(domainUser);


            var applicationUser = new ApplicationUser(domainUser.Email, domainUser.PhoneNumber);
            applicationUser.LinkToDomainUser(domainUser);

            var result = await _userManager.CreateAsync(applicationUser, password);

            if (!result.Succeeded)
            {
                return Error.Failure(description: "Failed to create user");
            }

            return applicationUser.Id;
        }


        public async Task<bool> isAssociatedWithDomainUser(int domainUserId, CancellationToken ct = default)
        {
            return await _userManager.Users.AnyAsync(au => au.DomainUserId == domainUserId, ct);
        }

        public async Task<ErrorOr<Success>> UpdateLinkedUser(int domainUserId, string? email, string? phoneNumber, CancellationToken ct = default)
        {
            var appUser = await _userManager.Users.FirstOrDefaultAsync(au => au.DomainUserId == domainUserId, ct);
            if (appUser is null)
            {
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found.");
            }

            if (!string.IsNullOrEmpty(email))
            {
                var emailExists = await _userManager.Users.AnyAsync(au => au.Email == email && au.Id != appUser.Id, ct);
                if (emailExists)
                {
                    return Error.Conflict(code: ErrorCodes.User.EmailAlreadyExists, description: "Email already exists");
                }

                appUser.Email = email;
                appUser.UserName = email;
            }


            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var phoneExists = await _userManager.Users.AnyAsync(au => au.PhoneNumber == phoneNumber && au.Id != appUser.Id, ct);
                if (phoneExists)
                {
                    return Error.Conflict(code: ErrorCodes.User.PhoneAlreadyExists, description: "Phone number already exists");
                }

                appUser.PhoneNumber = phoneNumber;
            }

            var updateResult = await _userManager.UpdateAsync(appUser);
            if (!updateResult.Succeeded)
            {
                return Error.Failure(description: updateResult.Errors.FirstOrDefault()?.Description ?? "Failed to update user");
            }

            return Result.Success;
        }


    }
}
