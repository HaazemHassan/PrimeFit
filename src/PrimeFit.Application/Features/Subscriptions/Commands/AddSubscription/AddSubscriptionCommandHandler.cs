using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Specifications.BranchPackages;
using PrimeFit.Domain.Repositories;
using PrimeFit.Domain.ServicesContracts;

namespace PrimeFit.Application.Features.Subscriptions.Commands.AddSubscription
{
    public class AddSubscriptionCommandHandler : IRequestHandler<AddSubscriptionCommand, ErrorOr<AddSubscriptionCommandResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubscriptionDomainService _subscriptionService;
        private readonly IMapper _mapper;
        private readonly TimeProvider _timeProvider;

        public AddSubscriptionCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, ISubscriptionDomainService subscriptionService, IMapper mapper, TimeProvider timeProvider)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _subscriptionService = subscriptionService;
            _mapper = mapper;
            _timeProvider = timeProvider;
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


            var packageSpec = new BranchPackageWithBranchSpec(request.PackageId);

            var package = await _unitOfWork.Packages.FirstOrDefaultAsync(packageSpec, cancellationToken);

            var branch = package?.Branch;

            if (package is null || branch is null || branch.Id != request.BranchId || branch.OwnerId != curUserId)
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

            var subscription = createSubResult.Value;
            return _mapper.Map<AddSubscriptionCommandResponse>(subscription);
        }

    }
}
