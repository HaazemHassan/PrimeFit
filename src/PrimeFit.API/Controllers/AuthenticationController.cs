using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PrimeFit.API.Common.Constants;
using PrimeFit.API.Common.Filters;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Features.Authentication.Commands.RefreshToken;
using PrimeFit.Application.Features.Authentication.Commands.RegisterOwner;
using PrimeFit.Application.Features.Authentication.Commands.RegisterUser;
using PrimeFit.Application.Features.Authentication.Commands.SignIn;
using PrimeFit.Application.Features.Authentication.Common;
using PrimeFit.Infrastructure.Common.Options;

namespace PrimeFit.API.Controllers
{

    /// <summary>
    /// Authentication controller for handling user login and token management
    /// </summary>
    public class AuthenticationController(JwtOptions jwtSettings, IClientContextService clientContextService) : BaseController
    {
        private readonly JwtOptions _jwtSettings = jwtSettings;
        private readonly IClientContextService _clientContextService = clientContextService;






        /// <summary>
        /// Register a new user account
        /// </summary>
        /// <param name="command">User registration details including username, email, password, and personal information</param>
        /// <returns>JWT token with user information if registration is successful</returns>
        /// <response code="201">User registered successfully and JWT token returned</response>
        /// <response code="400">Invalid input data or registration failed</response>
        /// <response code="409">User with the same email, username, or phone number already exists</response>
        /// <response code="403">User is already authenticated (anonymous only endpoint)</response>
        [HttpPost("register-user")]
        [AnonymousOnly]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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












        /// <summary>
        /// Authenticate a user with username and password
        /// </summary>
        /// <param name="command">Login credentials including username and password</param>
        /// <returns>JWT access token and user information. Refresh token is stored in HTTP-only cookie</returns>
        /// <response code="200">Login successful, returns access token and user details</response>
        /// <response code="401">Invalid username or password</response>
        /// <response code="403">User is already authenticated (anonymous only endpoint)</response>
        /// <response code="429">Too many login attempts, please try again later</response>
        /// <remarks>
        /// The refresh token is automatically stored in an HTTP-only cookie named 'refreshToken' for security.
        /// Rate limit: 5 attempts per minute per IP address.
        /// </remarks>
        [HttpPost("login")]
        [AnonymousOnly]
        [EnableRateLimiting("loginLimiter")]
        [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Login([FromBody] SignInWithPasswordCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);

            HandleRefreshToken(result.Value);
            return Ok(result.Value);
        }



        /// <summary>
        /// Refresh an expired access token using the refresh token
        /// </summary>
        /// <param name="command">The current (possibly expired) access token</param>
        /// <returns>New JWT access token and refresh token</returns>
        /// <response code="200">Token refreshed successfully, returns new access token</response>
        /// <response code="401">Invalid or expired refresh token</response>
        /// <response code="400">Invalid access token format</response>
        /// <remarks>
        /// The refresh token is automatically read from the 'refreshToken' HTTP-only cookie.
        /// A new refresh token is generated and stored in the cookie, while the old one is revoked.
        /// The access token in the request body can be expired, but must be valid in format.
        /// </remarks>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            if (_clientContextService.IsWebClient())
                command.RefreshToken = Request.Cookies["refreshToken"];

            if (command.RefreshToken is null)
                return Unauthorized("Invalid Refresh token.");

            var result = await Mediator.Send(command);

            if (result.IsError)
                return Problem(result.Errors);

            HandleRefreshToken(result.Value);
            return Ok(result.Value);
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

        //helpers
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
    }

}




