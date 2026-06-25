using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Messaging;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Domain.DomainEvents;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Authentication.Events
{
    internal sealed class UserRegisteredIntegrationEventHandler
        : INotificationHandler<UserRegisteredIntegrationEvent>
    {
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IUnitOfWork _unitOfWork;

        public UserRegisteredIntegrationEventHandler(
            IEmailVerificationService emailVerificationService,
            IUnitOfWork unitOfWork)
        {
            _emailVerificationService = emailVerificationService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UserRegisteredIntegrationEvent notification, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetAsync(u => u.Email == notification.Email, cancellationToken);

            if (user is null)
            {
                return;
            }

            var createCodeResult = await _emailVerificationService.CreateEmailConfirmationCode(user.Id, cancellationToken);

            if (createCodeResult.IsError)
            {
                var error = createCodeResult.FirstError;
                if (error.Type == ErrorType.Validation || error.Type == ErrorType.Conflict || error.Type == ErrorType.NotFound)
                {
                    return;
                }

                throw new Exception($"Failed to generate confirmation code: {error.Description}");
            }

            await _emailVerificationService.SendConfirmationEmailAsync(user, createCodeResult.Value);
        }
    }
}
