using ErrorOr;

namespace PrimeFit.Application.ServicesContracts.Infrastructure
{
    public interface IImageService
    {
        Task<ErrorOr<string>> UploadAsync(Stream fileStream, string fileName);
        Task<ErrorOr<Success>> DeleteAsync(string publicId);
    }
}
