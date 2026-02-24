using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ErrorOr;
using Microsoft.Extensions.Options;
using PrimeFit.Application.Common.DTOS;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;

namespace PrimeFit.Infrastructure.Storage
{
    public class CloudinaryImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryImageService(IOptions<CloudinaryOptions> settings)
        {
            var account = new Account(
                settings.Value.CloudName,
                settings.Value.ApiKey,
                settings.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        public async Task<ErrorOr<ImageUploadDTO>> UploadAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error is not null)
            {
                return ErrorOr.Error.Failure(
                    code: ErrorCodes.Cloudinary.ImageUploadFailed,
                    description: uploadResult.Error.Message
                );
            }

            var response = new ImageUploadDTO
            {
                PublicId = uploadResult.PublicId,
                SecureUrl = uploadResult.SecureUrl.ToString()
            };

            return response;
        }

        public async Task<ErrorOr<Success>> DeleteAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Error is not null)
            {
                return ErrorOr.Error.Failure(
                    code: ErrorCodes.Cloudinary.ImageDeleteFailed,
                    description: result.Error.Message
                );
            }

            return Result.Success;
        }
    }
}