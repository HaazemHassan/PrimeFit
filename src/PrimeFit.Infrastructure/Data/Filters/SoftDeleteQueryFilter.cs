using Microsoft.EntityFrameworkCore;
using PrimeFit.Domain.Entities.Contracts;
using System.Linq.Expressions;

namespace PrimeFit.Infrastructure.Data.Filters;

internal static class SoftDeleteQueryFilter
{
    internal static void Apply(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDeletableEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(GenerateIsDeletedFilter(entityType.ClrType));
            }
        }
    }

    private static LambdaExpression GenerateIsDeletedFilter(Type type)
    {
        var parameter = Expression.Parameter(type, "it");
        var property = Expression.Property(parameter, nameof(IFullyAuditableEntity.IsDeleted));
        var falseConstant = Expression.Constant(false);
        var body = Expression.Equal(property, falseConstant);

        return Expression.Lambda(body, parameter);
    }
}
