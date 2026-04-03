using ErrorOr;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Contracts.Infrastructure
{
    public interface IEmailVerificationService
    {
        Task<ErrorOr<string>> CreateEmailConfirmationCode(int domainUserId, CancellationToken ct = default);
        Task SendConfirmationEmailAsync(DomainUser user, string code);
        Task<ErrorOr<Success>> ConfirmEmail(int appUserId, string code, CancellationToken ct = default);
    }
}
