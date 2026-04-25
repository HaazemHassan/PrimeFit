using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Exceptions;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;
using System.Reflection;

namespace PrimeFit.Application.Security
{
    public sealed class AuthorizationBehavior<TRequest, TResponse>(
        IAuthorizationService authorizationService,
        ICurrentUserService _currentUserService)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IAuthorizedRequest
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {

            if (!_currentUserService.IsAuthenticated)
            {
                throw new UnauthorizedException();
            }

            if (_currentUserService.UserType == UserType.SuperAdmin)
            {
                return await next(cancellationToken);
            }


            var attributes = typeof(TRequest).GetCustomAttributes<AuthorizeAttribute>().ToList();

            if (attributes.Count == 0)
            {
                return await next(cancellationToken);
            }



            ErrorOr<Success>? lastError = null;

            foreach (var attribute in attributes)
            {
                var userTypes = attribute.UserTypes;
                var roles = attribute.Roles;
                var permissions = attribute.Permissions;

                var policies = string.IsNullOrWhiteSpace(attribute.Policy)
                    ? Array.Empty<string>()
                    : [attribute.Policy];

                var result = authorizationService.AuthorizeCurrentUser(
                    request,
                    [.. roles],
                    [.. permissions],
                    [.. policies],
                    [.. userTypes]);

                if (!result.IsError)
                {
                    return await next(cancellationToken);
                }

                lastError = result;
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