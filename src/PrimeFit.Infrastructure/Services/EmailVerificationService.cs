using ErrorOr;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.RepositoriesContracts;
using PrimeFit.Infrastructure.BackgroundJobs.Jobs;
using PrimeFit.Infrastructure.Common.Options;
using PrimeFit.Infrastructure.Data.Identity.Entities;

namespace PrimeFit.Infrastructure.Services
{
    internal class EmailVerificationService(
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IOtpService otpService,
        IOptions<EmailVerificationCodeOptions> emailVerificationCodeOptions,
        IEmailBodyBuilderService emailBodyBuilderService) : IEmailVerificationService
    {
        private readonly EmailVerificationCodeOptions _emailOptions = emailVerificationCodeOptions.Value;

        public async Task<ErrorOr<string>> CreateEmailConfirmationCode(int domainUserId, CancellationToken ct = default)
        {
            var appUser = await userManager.Users.FirstOrDefaultAsync(au => au.DomainUserId == domainUserId, cancellationToken: ct);

            if (appUser is null)
            {
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found.");
            }

            if (appUser.EmailConfirmed)
            {
                return Error.Validation(code: ErrorCodes.Authentication.EmailAlreadyConfirmed, description: "Email is already confirmed.");
            }

            var existingCode = await unitOfWork.VerificationCodes.GetAsync(v =>
                v.ApplicationUserId == appUser.Id &&
                v.Type == VerificationCodeType.EmailConfirmation &&
                v.Status == VerificationCodeStatus.Active, ct);

            if (existingCode is not null)
            {
                var elapsedSinceCreation = dateTimeProvider.UtcNow - existingCode.CreatedAt;
                if (elapsedSinceCreation < TimeSpan.FromMinutes(1))
                {
                    return Error.Failure(description: "Please retry after a minute.");
                }

                existingCode.Revoke();
            }

            var confirmEmailCode = otpService.Generate(length: _emailOptions.CodeLength);
            var codeExpiresAt = dateTimeProvider.UtcNow.AddMinutes(minutes: _emailOptions.EmailExpireInMinutes);
            var verificationCode = new VerificationCode(appUser.Id, confirmEmailCode, VerificationCodeType.EmailConfirmation, codeExpiresAt);

            await unitOfWork.VerificationCodes.AddAsync(verificationCode, ct);

            return confirmEmailCode;
        }

        public Task SendConfirmationEmailAsync(DomainUser user, string code)
        {
            string emailBody = emailBodyBuilderService.GenerateEmailBody("EmailConfirmation",
                new Dictionary<string, string>
                {
                    { "Name", user.FirstName },
                    { "Code", code },
                    { "Minutes", _emailOptions.EmailExpireInMinutes.ToString() },
                });

            BackgroundJob.Enqueue<SendEmailJob>(job => job.Execute(user.Email, "Email Confirmation", emailBody));

            return Task.CompletedTask;
        }

        public async Task<ErrorOr<Success>> ConfirmEmail(int domainUserId, string code, CancellationToken ct = default)
        {
            var appUser = await userManager.Users.FirstOrDefaultAsync(au => au.DomainUserId == domainUserId, cancellationToken: ct);

            if (appUser is null)
            {
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found.");
            }

            if (appUser.EmailConfirmed)
            {
                return Error.Validation(code: ErrorCodes.Authentication.EmailAlreadyConfirmed, description: "Email is already confirmed.");
            }

            var verificationCode = await unitOfWork.VerificationCodes.GetAsync(v =>
                v.ApplicationUserId == appUser.Id &&
                v.Type == VerificationCodeType.EmailConfirmation &&
                v.Status == VerificationCodeStatus.Active,
                ct);

            if (verificationCode is null)
            {
                return Error.NotFound(code: ErrorCodes.Authentication.InvalidEmailConfirmationCode, description: "Invalid code");
            }

            var usingCodeResult = verificationCode.TryUse(code);

            if (usingCodeResult.IsError)
            {
                return usingCodeResult.Errors;
            }

            appUser.EmailConfirmed = true;
            return Result.Success;
        }
    }
}
