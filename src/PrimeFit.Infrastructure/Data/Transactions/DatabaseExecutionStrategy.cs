using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PrimeFit.Domain.Contracts.Repositories;

namespace PrimeFit.Infrastructure.Data.Transactions;

internal class DatabaseExecutionStrategy(IExecutionStrategy _efExecutionStrategy) : IDatabaseExecutionStrategy
{

    public async Task<TResult> ExecuteAsync<TResult>(
        Func<Task<TResult>> operation,
        CancellationToken cancellationToken = default)
    {
        return await ExecutionStrategyExtensions.ExecuteAsync(
            _efExecutionStrategy,
            (ct) => operation(),
            cancellationToken);
    }

    public async Task ExecuteAsync(
        Func<Task> operation,
        CancellationToken cancellationToken = default)
    {
        await ExecutionStrategyExtensions.ExecuteAsync(
            _efExecutionStrategy,
            (ct) => operation(),
            cancellationToken);
    }
}
