using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Application.Specifications.BranchPackages;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;
using PrimeFit.Domain.ServicesContracts;

namespace PrimeFit.Application.Features.Members.Commands.CreateMemberWithSubscription
{
    public class CreateMemberWithSubscriptionCommandHandler : IRequestHandler<CreateMemberWithSubscriptionCommand, ErrorOr<CreateMemberWithSubscriptionCommandResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubscriptionDomainService _subscriptionService;
        private readonly IMapper _mapper;
        private readonly TimeProvider _timeProvider;
        private readonly IPhoneNumberService _phoneNumberService;

        public CreateMemberWithSubscriptionCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, ISubscriptionDomainService subscriptionService, IMapper mapper, TimeProvider timeProvider, IPhoneNumberService phoneNumberService)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _subscriptionService = subscriptionService;
            _mapper = mapper;
            _timeProvider = timeProvider;
            _phoneNumberService = phoneNumberService;
        }

        public async Task<ErrorOr<CreateMemberWithSubscriptionCommandResponse>> Handle(CreateMemberWithSubscriptionCommand request, CancellationToken cancellationToken)
        {
            int curUserId = _currentUserService.UserId!.Value;

            var user = await _unitOfWork.Users.GetAsync(u => u.Email == request.Email, cancellationToken);

            if (user is not null)
            {
                return Error.Validation(ErrorCodes.User.EmailAlreadyExists, "This email is used");
            }

            var phoneNumberNormalized = _phoneNumberService.Normalize(request.PhoneNumber!);
            user = await _unitOfWork.Users.GetAsync(u => u.PhoneNumber == phoneNumberNormalized, cancellationToken);

            if (user is not null)
            {
                return Error.Validation(ErrorCodes.User.PhoneAlreadyExists, "This phone number is used");
            }

            var packageSpec = new PackageWithBranchSpec(request.PackageId);

            var package = await _unitOfWork.Packages.FirstOrDefaultAsync(packageSpec, cancellationToken);

            var branch = package?.Branch;

            if (package is null || branch is null || branch.Id != request.BranchId || branch.OwnerId != curUserId)
            {
                return Error.Validation(description: "Package not found");
            }


            var newUser = new DomainUser(request.FirstName, request.LastName, request.Email, phoneNumberNormalized);

            await _unitOfWork.Users.AddAsync(newUser, cancellationToken);


            var createSubResult = await _subscriptionService.CreateSubscriptionAsync(newUser, branch, package, cancellationToken);

            if (createSubResult.IsError)
            {
                return createSubResult.Errors;
            }

            await _unitOfWork.Subscriptions.AddAsync(createSubResult.Value, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var subscription = createSubResult.Value;
            return _mapper.Map<CreateMemberWithSubscriptionCommandResponse>(subscription);
        }

    }
}
