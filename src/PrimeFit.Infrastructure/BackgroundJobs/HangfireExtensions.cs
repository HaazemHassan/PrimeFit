
using Hangfire;
using PrimeFit.Infrastructure.BackgroundJobs.Jobs;

namespace PrimeFit.Infrastructure.BackgroundJobs
{
    public static class HangfireExtensions
    {
        public static void RegisterRecurringJobs(this IRecurringJobManager manager)
        {
            manager.AddOrUpdate<RefreshTokensCleanupJob>(
                RefreshTokensCleanupJob.JobId,
                job => job.ExecuteAsync(),
                RefreshTokensCleanupJob.Schedule
            );

        }
    }
}

