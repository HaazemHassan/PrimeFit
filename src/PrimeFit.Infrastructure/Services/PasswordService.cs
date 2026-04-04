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
    internal class PasswordService(
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IOtpService otpService,
        IOptions<EmailVerificationCodeOptions> emailVerificationCodeOptions,
        IEmailBodyBuilderService emailBodyBuilderService) : IPasswordService
    {
        private readonly EmailVerificationCodeOptions _emailOptions = emailVerificationCodeOptions.Value;

        public async Task<ErrorOr<Success>> ChangePassword(int domainUserId, string currentPassword, string newPassword)
        {
            var appUser = await userManager.Users.FirstOrDefaultAsync(au => au.DomainUserId == domainUserId);

            if (appUser is null)
            {
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found.");
            }

            var result = await userManager.ChangePasswordAsync(appUser, currentPassword, newPassword);

            if (!result.Succeeded)
            {
                return Error.Unauthorized(
                    code: ErrorCodes.Authentication.InvalidPassword,
                    description: result.Errors.FirstOrDefault()?.Description ?? "Password change failed");
            }

            return Result.Success;
        }

        public async Task<ErrorOr<string>> CreatePasswordResetCode(int domainUserId, CancellationToken ct = default)
        {
            var appUser = await userManager.Users.FirstOrDefaultAsync(au => au.DomainUserId == domainUserId, cancellationToken: ct);

            if (appUser is null)
            {
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found.");
            }

            var existingCode = await unitOfWork.VerificationCodes.GetAsync(v =>
                v.ApplicationUserId == appUser.Id &&
                v.Type == VerificationCodeType.PasswordReset &&
                v.Status == VerificationCodeStatus.Active, ct);

            existingCode?.Revoke();

            var resetPasswordCode = otpService.Generate(length: _emailOptions.CodeLength);
            var codeExpiresAt = dateTimeProvider.UtcNow.AddMinutes(minutes: _emailOptions.EmailExpireInMinutes);
            var verificationCode = new VerificationCode(appUser.Id, resetPasswordCode, VerificationCodeType.PasswordReset, codeExpiresAt);

            await unitOfWork.VerificationCodes.AddAsync(verificationCode, ct);

            return resetPasswordCode;
        }

        public Task SendPasswordResetEmailAsync(DomainUser user, string code)
        {
            string emailBody = emailBodyBuilderService.GenerateEmailBody("PasswordReset",
                new Dictionary<string, string>
                {
                    { "Name", user.FirstName },
                    { "Code", code },
                    { "Minutes", _emailOptions.EmailExpireInMinutes.ToString() },
                });

            BackgroundJob.Enqueue<SendEmailJob>(job => job.Execute(user.Email, "Password Reset", emailBody));

            return Task.CompletedTask;
        }

        public async Task<ErrorOr<bool>> IsPasswordResetCodeValid(string email, string code, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByEmailAsync(email);

            if (appUser is null)
            {
                return false;
            }

            var verificationCode = await unitOfWork.VerificationCodes.GetAsync(v =>
                v.ApplicationUserId == appUser.Id &&
                v.Type == VerificationCodeType.PasswordReset &&
                v.Status == VerificationCodeStatus.Active &&
                v.Code == code,
                ct);

            if (verificationCode is null)
            {
                return false;
            }

            return verificationCode.isValid();
        }

        public async Task<ErrorOr<Success>> ResetPassword(string email, string code, string newPassword, CancellationToken ct = default)
        {
            var appUser = await userManager.FindByEmailAsync(email);

            if (appUser is null)
            {
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found.");
            }

            var verificationCode = await unitOfWork.VerificationCodes.GetAsync(v =>
                v.ApplicationUserId == appUser.Id &&
                v.Type == VerificationCodeType.PasswordReset &&
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

            var identityResetToken = await userManager.GeneratePasswordResetTokenAsync(appUser);
            var resetResult = await userManager.ResetPasswordAsync(appUser, identityResetToken, newPassword);

            if (!resetResult.Succeeded)
            {
                return Error.Validation(
                    code: ErrorCodes.Authentication.InvalidPassword,
                    description: resetResult.Errors.FirstOrDefault()?.Description ?? "Password reset failed");
            }

            return Result.Success;
        }
    }
}
