namespace PrimeFit.Domain.Common.Constants;

public static class ErrorCodes
{
    public static class User
    {
        public const string EmailAlreadyExists = "EMAIL_ALREADY_EXISTS";
        public const string PhoneAlreadyExists = "PHONE_ALREADY_EXISTS";
        public const string UserNotFound = "USER_NOT_FOUND";
        public const string EmailNotVerified = "EMAIL_NOT_VERIFIED";
    }

    public static class Authentication
    {
        public const string InvalidCredentials = "INVALID_CREDENTIALS";
        public const string InvalidAccessToken = "INVALID_ACCESS_TOKEN";
        public const string InvalidRefreshToken = "INVALID_REFRESH_TOKEN";
        public const string AlreadySignedOut = "ALREADY_SIGNED_OUT";
        public const string PasswordChangeFailed = "PASSWORD_CHANGE_FAILED";
    }

    public static class Authorization
    {
        public const string MissingPermissions = "MISSING_PERMISSIONS";
        public const string MissingRoles = "MISSING_ROLES";
        public const string NotAllowed = "NOT_ALLOWED";
        public const string InvalidRequest = "INVALID_REQUEST";
        public const string UnknownPolicy = "UNKNOWN_POLICY";
        public const string NotAuthenticated = "NOT_AUTHENTICATED";
    }

    public static class Branch
    {
        public const string BranchNotFound = "BRANCH_NOT_FOUND";
        public const string GovernorateNotFound = "GOVERNORATE_NOT_FOUND";
    }

    public static class WorkingHours
    {
        public const string InvalidWorkingHours = "INVALID_WORKING_HOURS";
        public const string ShiftDurationTooShort = "SHIFT_DURATION_TOO_SHORT";
    }
}
