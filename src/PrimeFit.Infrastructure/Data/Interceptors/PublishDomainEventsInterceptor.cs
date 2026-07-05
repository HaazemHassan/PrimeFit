using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using PrimeFit.Domain.Primitives;

namespace PrimeFit.Infrastructure.Data.Interceptors
{
    public sealed class PublishDomainEventsInterceptor
        : SaveChangesInterceptor
    {
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            DbContext? dbContext = eventData.Context;

            if (dbContext is null)
            {
                return await base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            var domainEvents = dbContext.ChangeTracker
                .Entries<IHasDomainEvents>()
                .Select(x => x.Entity)
                .SelectMany(entity =>
                {
                    var domainEvents = entity.DomainEvents.ToList();
                    entity.ClearDomainEvents();
                    return domainEvents;
                })
                .ToList();

            if (domainEvents.Count != 0)
            {
                var publisher = dbContext.GetService<IPublisher>();
                foreach (var domainEvent in domainEvents)
                {
                    await publisher.Publish(domainEvent, cancellationToken);
                }
            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
