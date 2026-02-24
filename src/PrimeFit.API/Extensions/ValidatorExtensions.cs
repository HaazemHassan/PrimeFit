namespace PrimeFit.API.Extensions
{
    using FluentValidation;
    using Microsoft.AspNetCore.Http;

    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, IFormFile> ApplyImageRules<T>(
            this IRuleBuilder<T, IFormFile> ruleBuilder,
            long minFileSizeInBytes = 50 * 1024,               // 50 KB
            long maxFileSizeInBytes = 5 * 1024 * 1024)         // 5 MB default
        {
            string[] allowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
            string[] allowedContentTypes = ["image/jpeg", "image/png", "image/webp"];



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
                    .WithMessage("Unsupported file extension. Only JPG, PNG, and WebP are allowed.")

                .Must(file =>
                {
                    return allowedContentTypes.Contains(file.ContentType);
                }).WithMessage("Invalid content type. Only image/jpeg, image/png, and image/webp are allowed.");
        }
    }
}