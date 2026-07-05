using PrimeFit.Domain.Common.Enums;
using ErrorOr;

namespace PrimeFit.Api.Requests.Authentication.SignInWithGoogle;

public class SignInWithGoogleRequest
{
        public string IdToken { get; set; }
        public UserType UserType { get; set; }
}

