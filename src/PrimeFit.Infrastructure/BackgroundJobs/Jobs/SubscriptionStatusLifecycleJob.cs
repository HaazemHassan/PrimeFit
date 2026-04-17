using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Infrastructure.Data;

namespace PrimeFit.Infrastructure.BackgroundJobs.Jobs
{
    /// <summary>
    /// Periodically processes subscription statuses:
    /// - Handles freeze expiry and auto-chaining
    /// - Expires finished subscriptions
    /// - Activates the next scheduled subscription
    /// </summary>
    public class SubscriptionStatusLifecycleJob
    {
        public const string JobId = "SubscriptionsStatusProcessingJob";
        public static readonly string Schedule = Cron.Daily();

        private const int BatchSize = 200;

        private readonly AppDbContext _dbContext;
        private readonly ILogger<SubscriptionStatusLifecycleJob> _logger;
        private readonly TimeProvider _timeProvider;

        public SubscriptionStatusLifecycleJob(
            ILogger<SubscriptionStatusLifecycleJob> logger,
            TimeProvider timeProvider,
            AppDbContext dbContext)
        {
            _logger = logger;
            _timeProvider = timeProvider;
            _dbContext = dbContext;
        }

        public async Task ExecuteAsync(CancellationToken ct = default)
        {
            var now = _timeProvider.GetUtcNow();

            while (true)
            {
                var batch = await FetchBatchAsync(now, ct);

                if (batch.Count == 0)
                    break;

                var expiredPairs = ProcessBatch(batch, now);

                if (expiredPairs.Count > 0)
                    await ActivateNextScheduledAsync(expiredPairs, now, ct);

                await _dbContext.SaveChangesAsync(ct);
                _dbContext.ChangeTracker.Clear();

                if (batch.Count < BatchSize)
                    break;
            }
        }

        private async Task<List<Subscription>> FetchBatchAsync(
            DateTimeOffset now,
            CancellationToken ct)
        {
            return await _dbContext.Subscriptions
                .Include(s => s.Freezes)
                .Where(s =>
                    s.NextProcessingDate != null &&
                    s.NextProcessingDate <= now &&
                    (s.Status == SubscriptionStatus.Active ||
                     s.Status == SubscriptionStatus.Frozen))
                .OrderBy(s => s.Id)
                .AsSplitQuery()
                .Take(BatchSize)
                .ToListAsync(ct);
        }

        private async Task ActivateNextScheduledAsync(
            List<(int UserId, int BranchId)> expiredPairs,
            DateTimeOffset now,
            CancellationToken ct)
        {
            var userIds = expiredPairs
                .Select(p => p.UserId)
                .Distinct()
                .ToList();

            var candidates = await _dbContext.Subscriptions
                .Where(s =>
                    s.Status == SubscriptionStatus.Scheduled &&
                    userIds.Contains(s.UserId))
                .ToListAsync(ct);

            var expiredSet = expiredPairs.ToHashSet();

            var nextScheduled = candidates
                .Where(s => expiredSet.Contains((s.UserId, s.BranchId)))
                .GroupBy(s => new { s.UserId, s.BranchId })
                .Select(g => g.OrderBy(x => x.CreatedAt).First());

            foreach (var scheduled in nextScheduled)
                scheduled.Activate(now);
        }

        private List<(int UserId, int BranchId)> ProcessBatch(
            List<Subscription> batch,
            DateTimeOffset now)
        {
            var expiredPairs = new List<(int UserId, int BranchId)>();

            foreach (var subscription in batch)
            {
                try
                {
                    if (subscription.ProcessLifecycle(now))
                        expiredPairs.Add((subscription.UserId, subscription.BranchId));
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error processing subscription {SubscriptionId}",
                        subscription.Id);

                    subscription.SetNextProcessingDate(now.AddMinutes(5));
                }
            }

            return expiredPairs;
        }
    }
}