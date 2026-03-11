using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;
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

        public async Task<ErrorOr<DomainUser>> AddUser(DomainUser domainUser, string password, UserRole? UserRole = default, CancellationToken ct = default)
        {
            var existingDomainUser = await _unitOfWork.Users.GetAsync(u => u.Email == domainUser.Email, ct);

            if (existingDomainUser is not null)
            {
                var isAuthUserLinked = await _userManager.Users.AnyAsync(au =>
                    au.DomainUserId == existingDomainUser.Id ||
                    au.DomainUser != null && (
                        au.DomainUser.Email == existingDomainUser.Email ||
                        au.DomainUser.PhoneNumber == existingDomainUser.PhoneNumber
                    ), ct);

                if (isAuthUserLinked)
                {
                    var emailConflict = await _userManager.Users.AnyAsync(au =>
                        au.DomainUser != null && au.DomainUser.Email == existingDomainUser.Email, ct);

                    return emailConflict
                        ? Error.Conflict(code: ErrorCodes.User.EmailAlreadyExists, description: "This Email already exists")
                        : Error.Conflict(code: ErrorCodes.User.PhoneAlreadyExists, description: "Phone number already exists");
                }

                existingDomainUser.UpdateInfo(domainUser.FirstName, domainUser.LastName, domainUser.PhoneNumber);
                domainUser = existingDomainUser;
            }
            else
            {
                var phoneExists = await _unitOfWork.Users.AnyAsync(u => u.PhoneNumber == domainUser.PhoneNumber, ct);

                if (phoneExists)
                {
                    return Error.Conflict(code: ErrorCodes.User.PhoneAlreadyExists, description: "Phone number already exists");

                }
            }

            var applicationUser = new ApplicationUser(domainUser.Email, domainUser.PhoneNumber);
            applicationUser.AssignDomainUser(domainUser);

            var createResult = await _userManager.CreateAsync(applicationUser, password);
            if (!createResult.Succeeded)
            {
                return Error.Failure(description: "Failed to create user");

            }


            return domainUser;
        }


        public async Task<bool> isAssociatedWithDomainUser(int domainUserId, CancellationToken ct = default)
        {
            return await _userManager.Users.AnyAsync(au => au.DomainUserId == domainUserId, ct);
        }


    }
}
