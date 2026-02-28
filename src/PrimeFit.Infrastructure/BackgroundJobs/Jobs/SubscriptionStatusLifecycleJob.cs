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
    /// - Expires finished subscriptions (Expire)
    /// - Handles automatic unfreezing (Unfreeze)
    /// - Activates the next scheduled subscription (Activate Scheduled)
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

                if (!batch.Any())
                    break;

                var context = await BuildProcessingContextAsync(batch, ct);

                var expiredPairs = ProcessBatch(batch, context, now);

                if (expiredPairs.Any())
                {
                    await ActivateNextScheduledAsync(expiredPairs, now, ct);
                }

                await _dbContext.SaveChangesAsync(ct);
                _dbContext.ChangeTracker.Clear();

                if (batch.Count < BatchSize)
                    break;
            }
        }

        // =====================================================================
        // Data Access
        // =====================================================================

        /// <summary>
        /// Fetches subscriptions due for processing
        /// </summary>
        private async Task<List<Subscription>> FetchBatchAsync(
            DateTimeOffset now,
            CancellationToken ct)
        {
            return await _dbContext.Subscriptions
                .Where(s =>
                    s.NextProcessingDate != null &&
                    s.NextProcessingDate <= now &&
                    s.Status != SubscriptionStatus.Expired &&
                    s.Status != SubscriptionStatus.Cancelled)
                .OrderBy(s => s.Id)
                .Take(BatchSize)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Fetches all data related to the batch at once to avoid N+1 issues
        /// </summary>
        private async Task<ProcessingContext> BuildProcessingContextAsync(
            List<Subscription> batch,
            CancellationToken ct)
        {
            var subscriptionIds = batch.Select(s => s.Id).ToList();

            // Fetch all freezes for the batch
            var freezes = await _dbContext.SubscriptionFreezes
                .Where(f => subscriptionIds.Contains(f.SubscriptionId))
                .ToListAsync(ct);

            return new ProcessingContext(freezes);
        }

        /// <summary>
        /// Activates the oldest scheduled subscription for each (User, Branch) whose subscription expired
        /// </summary>
        private async Task ActivateNextScheduledAsync(
            List<(int UserId, int BranchId)> expiredPairs,
            DateTimeOffset now,
            CancellationToken ct)
        {
            var userIds = expiredPairs.Select(p => p.UserId).Distinct().ToList();
            var branchIds = expiredPairs.Select(p => p.BranchId).Distinct().ToList();

            // Fetch the oldest scheduled for each (UserId, BranchId) at once
            var nextScheduled = await _dbContext.Subscriptions
                .Where(s =>
                    s.Status == SubscriptionStatus.Scheduled &&
                    userIds.Contains(s.UserId) &&
                    branchIds.Contains(s.BranchId))
                .GroupBy(s => new { s.UserId, s.BranchId })
                .Select(g => g.OrderBy(x => x.CreatedAt).First())
                .ToListAsync(ct);

            foreach (var scheduled in nextScheduled)
            {
                // Ensure this pair actually expired (since the query fetches all branchIds)
                bool pairExpired = expiredPairs.Any(p =>
                    p.UserId == scheduled.UserId &&
                    p.BranchId == scheduled.BranchId);

                if (!pairExpired)
                    continue;

                scheduled.Activate(now);
                scheduled.SetNextProcessingDate(scheduled.EndDate);
            }
        }

        // =====================================================================
        // Processing
        // =====================================================================

        /// <summary>
        /// Processes all subscriptions in the batch and returns the expired pairs
        /// </summary>
        private List<(int UserId, int BranchId)> ProcessBatch(
            List<Subscription> batch,
            ProcessingContext context,
            DateTimeOffset now)
        {
            var expiredPairs = new List<(int UserId, int BranchId)>();

            foreach (var subscription in batch)
            {
                try
                {
                    var freezes = context.GetFreezesFor(subscription.Id);

                    bool expired = ProcessSubscription(subscription, freezes, now);

                    if (expired)
                        expiredPairs.Add((subscription.UserId, subscription.BranchId));
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error processing subscription {Id}",
                        subscription.Id);

                    // Retry after 5 minutes instead of staying in an infinite loop
                    subscription.SetNextProcessingDate(now.AddMinutes(5));
                }
            }

            return expiredPairs;
        }

        /// <summary>
        /// Processes a single subscription and returns true if it expired
        /// </summary>
        private bool ProcessSubscription(
            Subscription subscription,
            List<SubscriptionFreeze> freezes,
            DateTimeOffset now)
        {
            // This job only works on Active subscriptions
            if (subscription.Status != SubscriptionStatus.Active)
                return false;

            // Is there an active freeze?
            var activeFreeze = freezes.FirstOrDefault(f => f.EndDate == null);

            if (activeFreeze != null)
            {
                return HandleActiveFreeze(subscription, activeFreeze, freezes, now);
            }

            return HandleExpiry(subscription, now);
        }

        /// <summary>
        /// Processes the freeze: unfreezes if the duration ended, postpones if still active
        /// </summary>
        private bool HandleActiveFreeze(
            Subscription subscription,
            SubscriptionFreeze activeFreeze,
            List<SubscriptionFreeze> freezes,
            DateTimeOffset now)
        {
            var freezeEnd = activeFreeze.StartDate.AddDays(activeFreeze.MaxDays);

            if (now >= freezeEnd)
            {
                // End the freeze and recalculate the end date
                activeFreeze.EndDate = freezeEnd;
                RecalculateEndDate(subscription, freezes);

                // Schedule the next processing on the new subscription end date
                subscription.SetNextProcessingDate(subscription.EndDate);
            }
            else
            {
                // Freeze is still active, check again later
                subscription.SetNextProcessingDate(freezeEnd);
            }

            return false; // Not Expired
        }

        /// <summary>
        /// Checks for subscription expiration
        /// </summary>
        private bool HandleExpiry(
            Subscription subscription,
            DateTimeOffset now)
        {
            if (subscription.EndDate == null || now < subscription.EndDate)
                return false;

            subscription.MarkExpired(now);          // Changes status to Expired
            subscription.SetNextProcessingDate(null); // Stop processing

            return true; // Expired occurred
        }

        // =====================================================================
        // Helpers
        // =====================================================================

        /// <summary>
        /// Recalculates the subscription end date after calculating total freeze days
        /// </summary>
        private void RecalculateEndDate(
            Subscription subscription,
            List<SubscriptionFreeze> freezes)
        {
            var totalFreezeDays = freezes
                .Where(f => f.EndDate != null)
                .Sum(f => (int)Math.Ceiling((f.EndDate!.Value - f.StartDate).TotalDays));

            var baseEnd = subscription.ActivationDate!.Value
                .AddMonths(subscription.DurationInMonths);

            subscription.SetEndDate(baseEnd.AddDays(totalFreezeDays));
        }
    }

    // =====================================================================
    // Supporting Types
    // =====================================================================

    /// <summary>
    /// Pre-prepared data for the batch instead of querying per subscription
    /// </summary>
    internal sealed class ProcessingContext
    {
        // Dictionary for O(1) lookup instead of O(n) each time
        private readonly Dictionary<int, List<SubscriptionFreeze>> _freezesBySubscriptionId;

        public ProcessingContext(List<SubscriptionFreeze> allFreezes)
        {
            _freezesBySubscriptionId = allFreezes
                .GroupBy(f => f.SubscriptionId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public List<SubscriptionFreeze> GetFreezesFor(int subscriptionId)
        {
            return _freezesBySubscriptionId.TryGetValue(subscriptionId, out var freezes)
                ? freezes
                : new List<SubscriptionFreeze>();
        }
    }
}