using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using PrimeFit.Domain.Repositories;
using PrimeFit.Domain.RepositoriesContracts;
using PrimeFit.Infrastructure.Data.Transactions;

namespace PrimeFit.Infrastructure.Data;

internal class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IServiceProvider _serviceProvider;
    private IDbContextTransaction? _transaction;


    public UnitOfWork(AppDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public IUserRepository Users => _serviceProvider.GetRequiredService<IUserRepository>();
    public IRefreshTokenRepository RefreshTokens => _serviceProvider.GetRequiredService<IRefreshTokenRepository>();
    public IBranchRepository Branches => _serviceProvider.GetRequiredService<IBranchRepository>();
    public IBranchImageRepository BranchImages => _serviceProvider.GetRequiredService<IBranchImageRepository>();
    public IBranchReviewRepository BranchReviews => _serviceProvider.GetRequiredService<IBranchReviewRepository>();
    public IBranchWorkingHourRepository BranchWorkingHours => _serviceProvider.GetRequiredService<IBranchWorkingHourRepository>();
    public IGovernorateRepository Governorates => _serviceProvider.GetRequiredService<IGovernorateRepository>();

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        return new DatabaseTransaction(_transaction);
    }

    public bool HasCurrentTransaction()
    {
        return _context.Database.CurrentTransaction != null;
    }

    public IDatabaseExecutionStrategy CreateExecutionStrategy()
    {
        var efStrategy = _context.Database.CreateExecutionStrategy();
        return new DatabaseExecutionStrategy(efStrategy);
    }
}
