using PrimeFit.Domain.Common.Enums;
using ErrorOr;

namespace PrimeFit.Api.Requests.Authentication.SignInWithPassword;

public class SignInWithPasswordRequest
{
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
}

