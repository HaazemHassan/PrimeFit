using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Exceptions;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using System.Reflection;

namespace PrimeFit.Application.Security
{
    public sealed class BranchAuthorizationBehavior<TRequest, TResponse>(
        ICurrentUserService currentUserService,
        IPolicyEnforcer policyEnforcer,
        IBranchAuthorizationService branchAuthorizationService)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBranchAuthorizedRequest
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {

            if (currentUserService.UserType == UserType.SuperAdmin)
            {
                return await next(cancellationToken);
            }

            var attributes = typeof(TRequest).GetCustomAttributes<BranchAuthorizeAttribute>().ToList();

            if (attributes.Count == 0)
            {
                return await next(cancellationToken);
            }

            var userId = currentUserService.UserId;
            if (userId == null)
            {
                throw new UnauthorizedException();
            }

            ErrorOr<Success>? lastError = null;

            foreach (var attribute in attributes)
            {
                var permissions = attribute.BranchPermissions;
                var roles = attribute.BranchRoles;
                var policy = attribute.Policy;

                var isAuthorized = true;

                // 1. Check Permissions using the BranchAuthService logic
                foreach (var permission in permissions)
                {
                    var result = await branchAuthorizationService.AuthorizeAsync(request.BranchId, permission, cancellationToken);
                    if (result.IsError)
                    {
                        lastError = result.FirstError;
                        isAuthorized = false;
                        break;
                    }
                }

                // 2. Check Roles
                if (isAuthorized && roles.Length > 0 && !currentUserService.HasAllRoles([.. roles]))
                {
                    lastError = Error.Forbidden(code: ErrorCodes.Authorization.MissingRoles, description: "User is missing required roles for taking this action");
                    isAuthorized = false;
                }

                // 3. Check Policy
                if (isAuthorized && !string.IsNullOrWhiteSpace(policy))
                {
                    var policyResult = policyEnforcer.Authorize(request, policy);
                    if (policyResult.IsError)
                    {
                        lastError = policyResult.Errors.First();
                        isAuthorized = false;
                    }
                }

                if (isAuthorized)
                {
                    return await next(cancellationToken);
                }
            }

            var error = lastError!.Value.FirstError;

            throw error.Type switch
            {
                ErrorType.Unauthorized => new UnauthorizedException(error.Description),
                ErrorType.Forbidden => new ForbiddenException(error.Description),
                _ => new ForbiddenException(error.Description)
            };
        }
    }
}
