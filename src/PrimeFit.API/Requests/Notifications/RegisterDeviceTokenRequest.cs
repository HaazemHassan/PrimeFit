using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.API.Requests.Notifications
{
    public class RegisterDeviceTokenRequest
    {
        public string Token { get; set; } = string.Empty;
        public DevicePlatform DevicePlatform { get; set; }
    }
}
