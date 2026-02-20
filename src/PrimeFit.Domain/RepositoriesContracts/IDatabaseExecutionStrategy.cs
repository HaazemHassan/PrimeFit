namespace PrimeFit.Domain.Repositories;

public interface IDatabaseExecutionStrategy
{
    Task<TResult> ExecuteAsync<TResult>(
        Func<Task<TResult>> operation,
        CancellationToken cancellationToken = default);

    Task ExecuteAsync(
        Func<Task> operation,
        CancellationToken cancellationToken = default);
}
