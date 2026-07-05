using Microsoft.AspNetCore.Authorization;
using PrimeFit.Api.Requests.Branches.AddBranchImage;
using PrimeFit.Api.Requests.Branches.UpdateBranchImage;
using PrimeFit.Api.Requests.Branches.GetBranchById;
using PrimeFit.Api.Requests.Branches.ActivateBranchImages;
using PrimeFit.Api.Requests.Branches.GetBranchStatistics;
using PrimeFit.Api.Requests.Branches.GetOwnerBranchesStatistics;
using PrimeFit.Api.Requests.Branches.UpdateBranchStatus;
using PrimeFit.Api.Requests.Branches.UpdateBasicDetails;
using PrimeFit.Api.Requests.Branches.UpdateLocationDetailsRequest;
using PrimeFit.Api.Requests.Branches.UpdateWorkingHoursRequest;
using PrimeFit.Api.Requests.BranchPackages.AddPackage;
using PrimeFit.Api.Requests.BranchPackages.UpdatePackage;
using PrimeFit.Api.Requests.BranchPackages.UpdatePackageStatus;
using PrimeFit.Api.Requests.BranchPackages.GetBranchPackagesForCustomers;
using PrimeFit.Api.Requests.Employees.GetBranchEmployees;
using PrimeFit.Api.Requests.Employees.UpdateEmployeeRequest;
using PrimeFit.Api.Requests.Subscriptions.AddSubscription;
using PrimeFit.Api.Requests.Subscriptions.CreateMemberWithSubscription;
using PrimeFit.Api.Requests.Subscriptions.GetBranchSubscriptions;
using PrimeFit.Api.Requests.Subscriptions.GetSubscriptionAttendanceHistory;
using PrimeFit.Api.Requests.Common.Pagination;
using PrimeFit.Api.Requests.Users.UpdateProfile;
using PrimeFit.Api.Requests.BranchReviews;
using PrimeFit.Api.Requests.BranchReviews.GetBranchReviewsRequest;
using PrimeFit.Api.Requests.Notifications;
using PrimeFit.Api.Requests.Payments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PrimeFit.API.Common.Constants;
using PrimeFit.API.Common.Filters;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Features.Authentication.Commands.ChangePassword;
using PrimeFit.Application.Features.Authentication.Commands.ConfirmEmail;
using PrimeFit.Application.Features.Authentication.Commands.Logout;
using PrimeFit.Application.Features.Authentication.Commands.RefreshToken;
using PrimeFit.Application.Features.Authentication.Commands.RegisterOwner;
using PrimeFit.Application.Features.Authentication.Commands.RegisterUser;
using PrimeFit.Application.Features.Authentication.Commands.ResendConfirmEmail;
using PrimeFit.Application.Features.Authentication.Commands.ResetPassword;
using PrimeFit.Application.Features.Authentication.Commands.SendResetPasswordEmail;
using PrimeFit.Application.Features.Authentication.Commands.SignIn;
using PrimeFit.Application.Features.Authentication.Commands.SignInWithGoogle;
using PrimeFit.Application.Features.Authentication.Common;
using PrimeFit.Application.Features.Authentication.Queries.ValidateResetPasswordCode;
using AutoMapper;
using PrimeFit.Api.Requests.Authentication.RegisterUser;
using PrimeFit.Api.Requests.Authentication.RegisterOwner;
using PrimeFit.Api.Requests.Authentication.SignInWithPassword;
using PrimeFit.Api.Requests.Authentication.SignInWithGoogle;
using PrimeFit.Api.Requests.Authentication.RefreshToken;
using PrimeFit.Api.Requests.Authentication.ConfirmEmail;
using PrimeFit.Api.Requests.Authentication.ValidateResetPasswordCode;
using PrimeFit.Api.Requests.Authentication.ResendConfirmationEmail;
using PrimeFit.Api.Requests.Authentication.SendResetPasswordEmail;
using PrimeFit.Api.Requests.Authentication.ResetPassword;
using PrimeFit.Api.Requests.Authentication.ChangePassword;
using PrimeFit.Api.Requests.Authentication.Logout;

namespace PrimeFit.API.Controllers
{
    public class AuthenticationController(IClientContextService clientContextService, IMapper _mapper) : BaseController
    {
        private readonly IClientContextService _clientContextService = clientContextService;



        [HttpPost("register-user")]
        [AnonymousOnly]

        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var command = _mapper.Map<RegisterUserCommand>(request);
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
        public async Task<IActionResult> RegisterAsOwner([FromBody] RegisterOwnerRequest request)
        {
            var command = _mapper.Map<RegisterOwnerCommand>(request);
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
        [EnableRateLimiting("loginLimiter")]
        [AnonymousOnly]

        public async Task<IActionResult> Login([FromBody] SignInWithPasswordRequest request)
        {
            var command = _mapper.Map<SignInWithPasswordCommand>(request);
            var result = await Mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);

            HandleRefreshToken(result.Value);
            return Ok(result.Value);
        }

        [HttpPost("google")]
        [EnableRateLimiting("loginLimiter")]
        [AllowAnonymous]
        public async Task<IActionResult> SignInWithGoogle([FromBody] SignInWithGoogleRequest request)
        {
            var command = _mapper.Map<SignInWithGoogleCommand>(request);
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);

            }

            HandleRefreshToken(result.Value);
            return Ok(result.Value);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var command = _mapper.Map<RefreshTokenCommand>(request);
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
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            var command = _mapper.Map<ConfirmEmailCommand>(request);
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);

            }

            return NoContent();
        }

        [HttpPost("validate-reset-password-code")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateResetPasswordCode([FromBody] ValidateResetPasswordCodeRequest request)
        {
            var query = _mapper.Map<ValidateResetPasswordCodeQuery>(request);
            var result = await Mediator.Send(query);
            if (result.IsError)
            {
                return Problem(result.Errors);

            }

            return Ok(result.Value);
        }



        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest request)
        {
            var command = _mapper.Map<ResendConfirmationEmailCommand>(request);
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);

            }

            return NoContent();
        }

        [HttpPost("send-reset-password-email")]
        [AnonymousOnly]
        public async Task<IActionResult> SendResetPasswordEmail([FromBody] SendResetPasswordEmailRequest request)
        {
            var command = _mapper.Map<SendResetPasswordEmailCommand>(request);
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return NoContent();
        }

        [HttpPost("reset-password")]
        [AnonymousOnly]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var command = _mapper.Map<ResetPasswordCommand>(request);
            var result = await Mediator.Send(command);
            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return NoContent();
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var command = _mapper.Map<ChangePasswordCommand>(request);
            var result = await Mediator.Send(command);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return NoContent();
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            var command = _mapper.Map<LogoutCommand>(request);
            if (_clientContextService.IsWebClient())
                command.RefreshToken = Request.Cookies["refreshToken"];

            if (command.RefreshToken is null)
                return Unauthorized("Refresh token is required");

            var result = await Mediator.Send(command);
            if (result.IsError)
                return Problem(result.Errors);

            Response.Cookies.Delete("refreshToken");
            return NoContent();
        }

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
