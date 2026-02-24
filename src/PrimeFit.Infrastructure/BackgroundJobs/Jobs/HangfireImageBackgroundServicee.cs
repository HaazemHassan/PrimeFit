using Hangfire;
using PrimeFit.Application.ServicesContracts.Infrastructure;

namespace PrimeFit.Infrastructure.BackgroundJobs.Jobs
{
    public class HangfireImageBackgroundServicee : IImageBackgroundService
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public HangfireImageBackgroundServicee(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }

        public void DeleteImage(string publicId)
        {
            _backgroundJobClient.Enqueue<ImageCleanupJob>(job => job.Process(publicId));
        }
    }
}

