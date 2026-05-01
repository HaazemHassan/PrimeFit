using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Entities.Contracts;

namespace PrimeFit.Infrastructure.Data.Interceptors;

public sealed class AuditingInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AuditingInterceptor(ICurrentUserService currentUserService, IDateTimeProvider dateTimeProvider)
    {
        _currentUserService = currentUserService;
        _dateTimeProvider = dateTimeProvider;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        ApplyAuditInfo(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyAuditInfo(DbContext context)
    {
        var userId = _currentUserService.UserId;
        var utcNow = _dateTimeProvider.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity is IHasCreationTime)
                {
                    entry.Property(nameof(IHasCreationTime.CreatedAt)).CurrentValue = utcNow;
                }

                if (entry.Entity is IHasCreator)
                {
                    entry.Property(nameof(IHasCreator.CreatedBy)).CurrentValue = userId;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                if (entry.Entity is IHasModificationTime)
                {
                    entry.Property(nameof(IHasModificationTime.UpdatedAt)).CurrentValue = utcNow;
                }

                if (entry.Entity is IHasModifier)
                {
                    entry.Property(nameof(IHasModifier.UpdatedBy)).CurrentValue = userId;
                }

                if (entry.Entity is IHasCreationTime)
                {
                    entry.Property(nameof(IHasCreationTime.CreatedAt)).IsModified = false;
                }

                if (entry.Entity is IHasCreator)
                {
                    entry.Property(nameof(IHasCreator.CreatedBy)).IsModified = false;
                }
            }
            else if (entry.Entity is ISoftDeletableEntity && entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;

                entry.Property(nameof(ISoftDeletableEntity.IsDeleted)).CurrentValue = true;
                entry.Property(nameof(ISoftDeletableEntity.DeletedAt)).CurrentValue = utcNow;
                entry.Property(nameof(ISoftDeletableEntity.DeletedBy)).CurrentValue = userId;
            }
        }
    }
}
