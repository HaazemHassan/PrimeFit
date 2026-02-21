using PrimeFit.Domain.Common.Enums;
using System.Text.Json.Serialization;

namespace PrimeFit.Application.Features.Users.Common
{
    public class BaseUserResponse
    {

        [JsonPropertyOrder(-1)]
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.Member;

    }
}
