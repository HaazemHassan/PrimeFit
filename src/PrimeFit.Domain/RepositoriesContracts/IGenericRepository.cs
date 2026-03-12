using Ardalis.Specification;
using System.Linq.Expressions;

namespace PrimeFit.Domain.Repositories;

public interface IGenericRepository<T> : IRepositoryBase<T> where T : class
{
    IQueryable<T> AsQueryable();

    Task<TDestination?> GetByIdAsync<TDestination>(int id, CancellationToken ct);
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
    Task<TDestination?> GetAsync<TDestination>(Expression<Func<T, bool>> predicate, CancellationToken ct);
    Task<TDestination?> GetAsync<TDestination>(ISpecification<T> spec, CancellationToken ct);

    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct);
    Task<decimal> AverageAsync(Expression<Func<T, decimal>> selector, ISpecification<T>? spec = default, CancellationToken? ct = default);
    Task<List<TDestination>> ListAsync<TDestination>(ISpecification<T> spec, CancellationToken ct);
}
