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
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Features.Payments.Commands.FulfillPayment;
using PrimeFit.Application.Features.Payments.Commands.InitializeSubscriptionPayment;
using PrimeFit.Application.Features.Payments.Queries.GetPaymentTransactionDetails;
using PrimeFit.Application.ServicesContracts.Infrastructure.Payments;
using System.Security.Cryptography;
using System.Text;

namespace PrimeFit.API.Controllers
{
    public class PaymentsController : BaseController
    {
        private readonly IPaymentService _PaymentService;
        private readonly ICurrentUserService _currentUserService;

        public PaymentsController(
            IPaymentService stripePaymentService,
            ICurrentUserService currentUserService)
        {
            _PaymentService = stripePaymentService;
            _currentUserService = currentUserService;
        }

        [HttpPost("initialize")]
        [Authorize]
        public async Task<IActionResult> Initialize([FromBody] InitializeSubscriptionPaymentRequest request)
        {
            var command = new InitializeSubscriptionPaymentCommand
            {
                UserId = _currentUserService.UserId!.Value,
                PackageId = request.PackageId,
                BranchId = request.BranchId
            };

            var result = await Mediator.Send(command);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpGet("{transactionId}")]
        [Authorize]
        public async Task<IActionResult> GetTransactionDetails([FromRoute] int transactionId)
        {
            var query = new GetPaymentTransactionDetailsQuery
            {
                TransactionId = transactionId,
                UserId = _currentUserService.UserId!.Value
            };

            var result = await Mediator.Send(query);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            return Ok(result.Value);
        }



        [HttpPost("webhook")]
        [AllowAnonymous]
        [DisableRateLimiting]
        public async Task<IActionResult> Webhook()
        {
            var payload = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signature = HttpContext.Request.Headers["Stripe-Signature"].ToString();

            if (string.IsNullOrEmpty(signature))
            {
                return BadRequest("Missing Stripe-Signature header.");
            }

            var webhookEvent = _PaymentService.VerifyAndParseWebhook(payload, signature);

            if (webhookEvent is null)
            {
                return BadRequest("Invalid webhook signature.");
            }

            switch (webhookEvent.EventType)
            {
                case "payment_intent.succeeded":

                var requestId = ConvertToGuid(webhookEvent.EventId);

                var command = new FulfillPaymentCommand(requestId)
                {
                    StripePaymentIntentId = webhookEvent.PaymentIntentId
                };

                var result = await Mediator.Send(command);

                if (result.IsError)
                {
                    return Problem(result.Errors);
                }

                break;

                default:
                // Ignore unsupported Stripe events
                break;
            }

            return Ok();
        }


        /// <summary>
        /// Converts a Stripe Event ID (string) to a deterministic Guid for idempotency.
        /// </summary>
        private static Guid ConvertToGuid(string eventId)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(eventId));
            var guidBytes = new byte[16];
            Array.Copy(hash, guidBytes, 16);
            return new Guid(guidBytes);
        }
    }
}
