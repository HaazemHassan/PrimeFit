using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Notifications.Commands.RegisterDeviceToken
{
    internal sealed class RegisterDeviceTokenCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
        : IRequestHandler<RegisterDeviceTokenCommand, ErrorOr<Success>>
    {
        public async Task<ErrorOr<Success>> Handle(RegisterDeviceTokenCommand request, CancellationToken ct)
        {
            var userId = currentUserService.UserId;

            if (userId is null) 
            {
                return Error.Unauthorized(description: "User is not authenticated.");
            }

            var existingToken = await unitOfWork.UserDeviceTokens.GetByTokenAsync(request.Token, ct);

            if (existingToken is not null)
            {
                existingToken.UpdateOwnership(userId.Value, request.DevicePlatform);
            }
            else
            {
                var newToken = new UserDeviceToken(userId.Value, request.Token, request.DevicePlatform);
                await unitOfWork.UserDeviceTokens.AddAsync(newToken, ct);
            }

            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success;
        }
    }
}
