using PrimeFit.Domain.Entities.Contracts;
using PrimeFit.Domain.Primitives.PrimeFit.Domain.Primitives;

namespace PrimeFit.Domain.Entities
{
    public sealed class RefreshToken : BaseEntity<int>, IHasCreationTime
    {
        public RefreshToken(string token, DateTimeOffset expires, string accessTokenJTI, int userId)
        {
            Token = token;
            Expires = expires;
            AccessTokenJTI = accessTokenJTI;
            UserId = userId;
        }

        public int UserId { get; set; }      //ApplicationUser not DomainUser
        public string AccessTokenJTI { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTimeOffset Expires { get; set; }
        public bool IsExpired => DateTimeOffset.UtcNow >= Expires;
        public DateTimeOffset? RevokationDate { get; set; }
        public bool IsActive => RevokationDate is null && !IsExpired;

        public DateTimeOffset CreatedAt { get; set; }

        public void Revoke()
        {
            RevokationDate = DateTimeOffset.UtcNow;
        }
    }
}
