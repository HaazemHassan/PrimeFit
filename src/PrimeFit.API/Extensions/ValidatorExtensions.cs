namespace PrimeFit.API.Extensions
{
    using FluentValidation;
    using Microsoft.AspNetCore.Http;

    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, IFormFile> ApplyImageRules<T>(
            this IRuleBuilder<T, IFormFile> ruleBuilder,
            long minFileSizeInBytes = 5 * 1024,
            long maxFileSizeInBytes = 5 * 1024 * 1024)
        {
            string[] allowedExtensions = [".jpg", ".jpeg", ".png", ".webp", ".gif", ".bmp", ".heic", ".heif", ".tiff", ".tif"];
            string[] allowedContentTypes = [
                "image/jpeg", "image/pjpeg",
                "image/png", "image/x-png",
                "image/webp",
                "image/gif",
                "image/bmp", "image/x-bmp",
                "image/heic", "image/heif",
                "image/tiff"
            ];



            return ruleBuilder
                .NotEmpty()
                    .WithMessage("Image file is required.")

                .Must(file => file != null && file.Length > 0)
                    .WithMessage("Image file cannot be empty.")

                .Must(file => file.Length >= minFileSizeInBytes)
                    .WithMessage($"Image size is too small. Minimum allowed size is {minFileSizeInBytes / 1024} KB.")

                .Must(file => file.Length <= maxFileSizeInBytes)
                    .WithMessage($"Image size exceeds the maximum limit of {maxFileSizeInBytes / 1024 / 1024} MB.")

                .Must(file =>
                {
                    var extension = Path.GetExtension(file.FileName)?.ToLower();
                    return !string.IsNullOrWhiteSpace(extension) &&
                           allowedExtensions.Contains(extension);
                })
                    .WithMessage("Unsupported file extension. Allowed extensions are: JPG, JPEG, PNG, WebP, GIF, BMP, HEIC, HEIF, TIFF.")

                .Must(file =>
                {
                    return allowedContentTypes.Contains(file.ContentType);
                }).WithMessage("Invalid content type. Allowed image formats are: JPG, JPEG, PNG, WebP, GIF, BMP, HEIC, HEIF, TIFF.");
        }
    }
}