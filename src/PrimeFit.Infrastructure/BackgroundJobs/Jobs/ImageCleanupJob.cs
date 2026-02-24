using PrimeFit.Application.ServicesContracts.Infrastructure;

namespace PrimeFit.Infrastructure.BackgroundJobs.Jobs
{
    public class ImageCleanupJob
    {
        private readonly IImageService _imageService;

        public ImageCleanupJob(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task Process(string publicId)
        {
            var result = await _imageService.DeleteAsync(publicId);

            if (result.IsError)
            {
                throw new Exception($"Failed to delete image from cloud. Error: {result.FirstError.Description}");
            }
        }
    }
}
