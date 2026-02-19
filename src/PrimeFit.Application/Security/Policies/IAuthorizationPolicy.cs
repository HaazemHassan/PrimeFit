using ErrorOr;

namespace PrimeFit.Application.Security.Policies
{
    public interface IAuthorizationPolicy
    {
        string Name { get; }
        ErrorOr<Success> Authorize(object request);
    }
}
