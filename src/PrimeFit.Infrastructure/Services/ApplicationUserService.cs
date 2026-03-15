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


    }
}
