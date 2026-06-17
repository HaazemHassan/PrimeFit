using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities.Base;

namespace PrimeFit.Domain.Entities
{
    public sealed class UserDeviceToken : AuditableEntity<int>
    {
        public UserDeviceToken(int userId, string token, DevicePlatform devicePlatform)
        {
            UserId = userId;
            Token = token;
            DevicePlatform = devicePlatform;
        }

        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DevicePlatform DevicePlatform { get; set; }

        public void UpdateOwnership(int userId, DevicePlatform devicePlatform)
        {
            UserId = userId;
            DevicePlatform = devicePlatform;
        }
    }
}
