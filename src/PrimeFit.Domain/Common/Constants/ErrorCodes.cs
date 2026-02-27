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
        public const string LogoAlreadyExists = "BRANCH_LOGO_ALREADY_EXISTS";
        public const string ImageNotFound = "BRANCH_IMAGE_NOT_FOUND";
        public const string ImagesCountLimitExceeded = "BRANCH_IMAGES_COUNT_LIMIT_EXCEEDED";
        public const string PackageNotFound = "PACKAGE_NOT_FOUND";
        public const string InvalidStatusTransition = "BRANCH_INVALID_STATUS_TRANSITION";


        // Activation / Completeness Errors
        public const string NameRequired = "BRANCH_NAME_REQUIRED";
        public const string EmailRequired = "BRANCH_EMAIL_REQUIRED";
        public const string PhoneRequired = "BRANCH_PHONE_REQUIRED";
        public const string GovernorateRequired = "BRANCH_GOVERNORATE_REQUIRED";
        public const string AddressRequired = "BRANCH_ADDRESS_REQUIRED";
        public const string LocationRequired = "BRANCH_LOCATION_REQUIRED";
        public const string WorkingHoursRequired = "BRANCH_WORKING_HOURS_REQUIRED";
        public const string LogoRequired = "BRANCH_LOGO_REQUIRED";
        public const string MarketPlaceImagesRequired = "BRANCH_MARKETPLACE_IMAGES_REQUIRED";

    }

    public static class WorkingHours
    {
        public const string InvalidWorkingHours = "INVALID_WORKING_HOURS";
        public const string ShiftDurationTooShort = "SHIFT_DURATION_TOO_SHORT";
    }

    public static class Cloudinary
    {
        public const string ImageUploadFailed = "IMAGE_UPLOAD_FAILED";
        public const string ImageDeleteFailed = "IMAGE_DELETE_FAILED";
    }

}
