using Hangfire;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Infrastructure.Storage.Jobs;

namespace PrimeFit.Infrastructure.Storage
{
    public class HangfireImageBackgroundService : IImageBackgroundService
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public HangfireImageBackgroundService(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }

        public void DeleteImage(string publicId)
        {
            _backgroundJobClient.Enqueue<ImageCleanupJob>(job => job.Process(publicId));
        }
    }
}

