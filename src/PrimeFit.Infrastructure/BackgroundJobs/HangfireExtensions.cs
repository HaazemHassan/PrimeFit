
using Hangfire;
using PrimeFit.Infrastructure.BackgroundJobs.Jobs;

namespace PrimeFit.Infrastructure.BackgroundJobs
{
    public static class HangfireExtensions
    {
        public static void RegisterRecurringJobs(this IRecurringJobManager manager, OutboxOptions outboxOptions)
        {
            manager.AddOrUpdate<RefreshTokensCleanupJob>(
                RefreshTokensCleanupJob.JobId,
                job => job.ExecuteAsync(),
                RefreshTokensCleanupJob.Schedule
            );


            manager.AddOrUpdate<SubscriptionStatusLifecycleJob>(
                SubscriptionStatusLifecycleJob.JobId,
                job => job.ExecuteAsync(CancellationToken.None),
                SubscriptionStatusLifecycleJob.Schedule
            );


            manager.AddOrUpdate<OrphanedImagesCleanupJob>(
                OrphanedImagesCleanupJob.JobId,
                job => job.ExecuteAsync(CancellationToken.None),
                OrphanedImagesCleanupJob.Schedule
            );

            manager.AddOrUpdate<InactiveUsersNotificationJob>(
                InactiveUsersNotificationJob.JobId,
                job => job.ExecuteAsync(CancellationToken.None),
                InactiveUsersNotificationJob.Schedule
            );

            string outboxSchedule = outboxOptions.ProcessingIntervalSeconds < 60
                ? $"*/{outboxOptions.ProcessingIntervalSeconds} * * * * *"
                : $"*/{outboxOptions.ProcessingIntervalSeconds / 60} * * * *";

            manager.AddOrUpdate<ProcessOutboxMessagesJob>(
                ProcessOutboxMessagesJob.JobId,
                job => job.ExecuteAsync(CancellationToken.None),
                outboxSchedule
            );

        }
    }
}

