using System.Reflection;
using System.Text.Json;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PrimeFit.Application.Common.Messaging;
using PrimeFit.Infrastructure.BackgroundJobs;
using PrimeFit.Infrastructure.Data;
using PrimeFit.Infrastructure.Data.Outbox;

namespace PrimeFit.Infrastructure.Messaging.Jobs
{
    public class ProcessOutboxMessagesJob
    {
        public const string JobId = "ProcessOutboxMessagesJob";

        private readonly AppDbContext _dbContext;
        private readonly IPublisher _publisher;
        private readonly ILogger<ProcessOutboxMessagesJob> _logger;
        private readonly OutboxOptions _options;

        public ProcessOutboxMessagesJob(
            AppDbContext dbContext,
            IPublisher publisher,
            ILogger<ProcessOutboxMessagesJob> logger,
            IOptions<OutboxOptions> options)
        {
            _dbContext = dbContext;
            _publisher = publisher;
            _logger = logger;
            _options = options.Value;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            List<OutboxMessage> messages = await _dbContext
                .Set<OutboxMessage>()
                .Where(m => m.ProcessedOnUtc == null)
                .OrderBy(m => m.OccurredOnUtc)
                .Take(_options.BatchSize)
                .ToListAsync(cancellationToken);

            foreach (OutboxMessage outboxMessage in messages)
            {
                try
                {
                    IIntegrationEvent? integrationEvent = DeserializeIntegrationEvent(outboxMessage);

                    if (integrationEvent is null)
                    {
                        _logger.LogWarning(
                            "Could not deserialize outbox message {MessageId} of type {Type}",
                            outboxMessage.Id,
                            outboxMessage.Type);
                        continue;
                    }

                    await _publisher.Publish(integrationEvent, cancellationToken);

                    outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error processing outbox message {MessageId}",
                        outboxMessage.Id);

                    outboxMessage.Error = ex.Message;
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private static IIntegrationEvent? DeserializeIntegrationEvent(OutboxMessage outboxMessage)
        {
            var integrationEventsAssembly = typeof(IIntegrationEvent).Assembly;

            var type = integrationEventsAssembly
                .GetTypes()
                .FirstOrDefault(t => t.Name == outboxMessage.Type);

            if (type is null)
            {
                return null;
            }

            var integrationEvent = JsonSerializer.Deserialize(outboxMessage.Content, type) as IIntegrationEvent;

            return integrationEvent;
        }
    }
}
