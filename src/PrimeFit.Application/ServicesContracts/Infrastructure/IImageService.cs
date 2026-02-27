using ErrorOr;
using PrimeFit.Application.Common.DTOS;

namespace PrimeFit.Application.ServicesContracts.Infrastructure
{
    public interface IImageService
    {
        Task<ErrorOr<ImageUploadDTO>> UploadAsync(Stream fileStream, string fileName, CancellationToken ct);
        Task<ErrorOr<ImageUploadDTO>> ReplaceAsync(Stream fileStream, string existingPublicId, string fileName, CancellationToken ct);

        Task<ErrorOr<Success>> DeleteAsync(string publicId);

    }
}
