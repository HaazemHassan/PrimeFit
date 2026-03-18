using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeFit.API.Common.Constants;
using PrimeFit.API.Common.Filters;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Features.Authentication.Commands.ConfirmEmail;
using PrimeFit.Application.Features.Authentication.Commands.RefreshToken;
using PrimeFit.Application.Features.Authentication.Commands.RegisterOwner;
using PrimeFit.Application.Features.Authentication.Commands.RegisterUser;
using PrimeFit.Application.Features.Authentication.Commands.ResendConfirmEmail;
using PrimeFit.Application.Features.Authentication.Commands.SignIn;
using PrimeFit.Application.Features.Authentication.Common;

namespace PrimeFit.API.Controllers
{

    public class AuthenticationController(IClientContextService clientContextService) : BaseController
    {
        private readonly IClientContextService _clientContextService = clientContextService;



        [HttpPost("register-user")]
        [AnonymousOnly]

        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await Mediator.Send(command);

            if (result.IsError)
                return Problem(result.Errors);

            return CreatedAtRoute(
                            routeName: RouteNames.Users.GetUserById,
                            routeValues: new { id = result.Value!.Id },
                            value: result.Value
             );


        }

        [HttpPost("register-owner")]
        public async Task<IActionResult> RegisterAsOwner([FromBody] RegisterOwnerCommand command)
        {
            var result = await Mediator.Send(command);

            if (result.IsError)
                return Problem(result.Errors);

            return CreatedAtRoute(
                            routeName: RouteNames.Users.GetUserById,
                            routeValues: new { id = result.Value!.Id },
                            value: result.Value
             );

        }



        [HttpPost("login")]
        [AnonymousOnly]

        public async Task<IActionResult> Login([FromBody] SignInWithPasswordCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);

            HandleRefreshToken(result.Value);
            return Ok(result.Value);
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            if (_clientContextService.IsWebClient())
                command.RefreshToken = Request.Cookies["refreshToken"];

            if (command.RefreshToken is null)
                return Unauthorized("Invalid Refresh token.");

            var result = await Mediator.Send(command);

            if (result.IsError)
            {
                return Problem(result.Errors);

            }

            HandleRefreshToken(result.Value);
            return Ok(result.Value);
        }

        [HttpPost("confirm-email")]
        [Authorize]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);

            }

            return NoContent();
        }



        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);

            }

            return NoContent();
        }

        //[HttpPost("logout")]
        //[Authorize]
        //public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
        //{
        //    if (_clientContextService.IsWebClient())
        //        command.RefreshToken = Request.Cookies["refreshToken"];

        //    if (command.RefreshToken is null)
        //        return Unauthorized("Refresh token is required");

        //    var result = await Mediator.Send(command);
        //    if (result.IsError)
        //        return Problem(result.Errors);

        //    Response.Cookies.Delete("refreshToken");
        //    return NoContent();
        //}

        #region Helpers
        private void HandleRefreshToken(AuthResult authResult)
        {
            if (authResult?.RefreshToken is null)
                return;

            var refreshToken = authResult.RefreshToken.Token;

            if (_clientContextService.IsWebClient())
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Path = "/api/authentication",
                    Expires = authResult.RefreshToken.ExpirationDate
                };
                Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
                authResult.RefreshToken = null;
            }
        }
        #endregion
    }

}




