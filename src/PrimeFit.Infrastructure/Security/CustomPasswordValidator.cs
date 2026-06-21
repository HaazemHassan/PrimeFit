using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PrimeFit.Application.Common.Options;
using PrimeFit.Infrastructure.Data.Identity.Entities;

namespace PrimeFit.Infrastructure.Security
{
    internal class CustomPasswordValidator : IPasswordValidator<ApplicationUser>
    {
        private readonly AppPasswordOptions _options;

        public CustomPasswordValidator(IOptions<AppPasswordOptions> options)
        {
            _options = options.Value;
        }

        public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string? password)
        {
            var errors = new List<IdentityError>();

            if (_options.RequireDigitOrNonAlphanumeric)
            {
                if (string.IsNullOrEmpty(password) || !password.Any(c => char.IsDigit(c) || !char.IsLetterOrDigit(c)))
                {
                    errors.Add(new IdentityError
                    {
                        Code = "PasswordRequiresDigitOrSymbol",
                        Description = "Passwords must have at least one number or symbol."
                    });
                }
            }

            return Task.FromResult(errors.Count == 0
                ? IdentityResult.Success
                : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
