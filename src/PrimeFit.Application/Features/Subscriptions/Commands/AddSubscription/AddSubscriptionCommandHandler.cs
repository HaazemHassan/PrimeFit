using Ardalis.Specification;
using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.DTOS;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Application.Specifications.BranchPackages;
using PrimeFit.Domain.RepositoriesContracts;
using PrimeFit.Domain.ServicesContracts;

namespace PrimeFit.Application.Features.Subscriptions.Commands.AddSubscription
{
    public class AddSubscriptionCommandHandler : IRequestHandler<AddSubscriptionCommand, ErrorOr<AddSubscriptionCommandResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubscriptionDomainService _subscriptionService;
        private readonly IMapper _mapper;
        private readonly IPushNotificationService _pushNotificationService;

        public AddSubscriptionCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, ISubscriptionDomainService subscriptionService, IMapper mapper, IPushNotificationService pushNotificationService)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _subscriptionService = subscriptionService;
            _mapper = mapper;
            _pushNotificationService = pushNotificationService;
        }

        public async Task<ErrorOr<AddSubscriptionCommandResponse>> Handle(AddSubscriptionCommand request, CancellationToken cancellationToken)
        {

            int curUserId = _currentUserService.UserId!.Value;

            var user = await _unitOfWork.Users.GetAsync(u => u.Email == request.Email, cancellationToken);

            if (user is null)
            {
                return Error.Validation(description: "User not found");
            }


            if (curUserId == user.Id)
            {
                return Error.Validation(description: "You can't add subscription to yourself");
            }


            var packageSpec = new PackageWithBranchSpec(request.PackageId);

            var package = await _unitOfWork.Packages.FirstOrDefaultAsync(packageSpec, cancellationToken);

            var branch = package?.Branch;

            if (package is null || branch is null || branch.Id != request.BranchId)
            {
                return Error.Validation(description: "Package not found");
            }


            var createSubResult = await _subscriptionService.CreateSubscriptionAsync(user, branch, package, cancellationToken);

            if (createSubResult.IsError)
            {
                return createSubResult.Errors;
            }

            await _unitOfWork.Subscriptions.AddAsync(createSubResult.Value, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var tokensSpec = new TokensByUserIdSpec(user.Id);
            var deviceTokens = await _unitOfWork.UserDeviceTokens.ListAsync(tokensSpec, cancellationToken);
            var tokens = deviceTokens.Select(t => t.Token).ToList();

            if (tokens.Count > 0)
            {
                var notificationRequest = new PushNotificationRequest
                {
                    Title = "PrimeFit",
                    Body = $"تم إضافة اشتراك جديد لك في فرع {branch.Name}"
                };

                await _pushNotificationService.SendToDevicesAsync(notificationRequest, tokens, cancellationToken);
            }

            var subscription = createSubResult.Value;
            return _mapper.Map<AddSubscriptionCommandResponse>(subscription);
        }

    }
}
