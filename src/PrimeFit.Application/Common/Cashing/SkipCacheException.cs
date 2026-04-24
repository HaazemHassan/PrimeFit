using ErrorOr;

namespace PrimeFit.Application.Common.Cashing;

internal class SkipCacheException<TValue>(ErrorOr<TValue> response) : Exception
{
    public ErrorOr<TValue> Response { get; } = response;
}