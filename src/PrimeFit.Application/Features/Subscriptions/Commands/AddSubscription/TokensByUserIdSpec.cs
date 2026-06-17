using Ardalis.Specification;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Subscriptions.Commands.AddSubscription
{
    public class TokensByUserIdSpec : Specification<UserDeviceToken>
    {
        public TokensByUserIdSpec(int userId)
        {
            Query.Where(t => t.UserId == userId);
        }
    }
}
