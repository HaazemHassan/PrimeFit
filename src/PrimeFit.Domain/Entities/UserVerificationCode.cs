using ErrorOr;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities.Contracts;
using PrimeFit.Domain.Primitives.PrimeFit.Domain.Primitives;

namespace PrimeFit.Domain.Entities
{
    public class VerificationCode : BaseEntity<int>, IHasCreationTime
    {
        public static readonly int MaxAttempts = 3;


        private VerificationCode() { }

        public VerificationCode(
            int applicationUserId,
            string code,
            VerificationCodeType type,
            DateTimeOffset expiresAt)
        {
            ApplicationUserId = applicationUserId;
            Code = code;
            Type = type;
            ExpiresAt = expiresAt;
            Attempts = 0;
            Status = VerificationCodeStatus.Active;

        }

        public int ApplicationUserId { get; private set; } = default!;

        // public string CodeHash { get; private set; } = default!;
        public string Code { get; private set; } = default!;

        public VerificationCodeType Type { get; private set; }
        public VerificationCodeStatus Status { get; private set; }

        public DateTimeOffset ExpiresAt { get; private set; }

        public int Attempts { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }



        public ErrorOr<Success> IncrementAttempts()
        {

            if (Status != VerificationCodeStatus.Active)
            {
                return Error.Validation(description: "Some thing went wrong");
            }


            if (Attempts >= MaxAttempts)
            {
                MarkAsRevoked();
                return Error.Validation(code: ErrorCodes.Authentication.EmailCodeAttemptsExceeded, description: "Some thing went wrong");
            }

            Attempts++;
            return Result.Success;
        }



        public void MarkAsUsed()
        {
            if (Status != VerificationCodeStatus.Active)
                throw new InvalidOperationException("Invalid code status to use this code");

            Status = VerificationCodeStatus.Used;
        }

        public void MarkAsRevoked()
        {
            if (Status != VerificationCodeStatus.Active)
                throw new InvalidOperationException("Invalid code status to revoke this code");

            Status = VerificationCodeStatus.Revoked;
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow > ExpiresAt;
        }

        public bool CanBeUsed()
        {
            return !IsExpired() && Attempts < MaxAttempts && Status == VerificationCodeStatus.Active;
        }
    }
}