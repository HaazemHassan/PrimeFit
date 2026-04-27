using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Application.Specifications.BranchPackages;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.RepositoriesContracts;
using PrimeFit.Domain.ServicesContracts;

namespace PrimeFit.Application.Features.Branches.Commands.CreateMemberWithSubscription
{
    public class CreateMemberWithSubscriptionCommandHandler : IRequestHandler<CreateMemberWithSubscriptionCommand, ErrorOr<CreateMemberWithSubscriptionCommandResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubscriptionDomainService _subscriptionService;
        private readonly IMapper _mapper;
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly ITotpService _totpService;

        public CreateMemberWithSubscriptionCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, ISubscriptionDomainService subscriptionService, IMapper mapper, IPhoneNumberService phoneNumberService, ITotpService totpService)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _subscriptionService = subscriptionService;
            _mapper = mapper;
            _phoneNumberService = phoneNumberService;
            _totpService = totpService;
        }

        public async Task<ErrorOr<CreateMemberWithSubscriptionCommandResponse>> Handle(CreateMemberWithSubscriptionCommand request, CancellationToken cancellationToken)
        {


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

            if (package is null || branch is null || branch.Id != request.BranchId)
            {
                return Error.Validation(description: "Package not found");
            }



            var totpSecret = _totpService.GenerateTotpSecret();
            var newUser = new DomainUser(UserType.Customer, request.FirstName, request.LastName, request.Email, phoneNumberNormalized, totpSecret);



            //Add

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            await _unitOfWork.Users.AddAsync(newUser, cancellationToken);


            var createSubResult = await _subscriptionService.CreateSubscriptionAsync(newUser, branch, package, cancellationToken);

            if (createSubResult.IsError)
            {
                return createSubResult.Errors;
            }

            await _unitOfWork.Subscriptions.AddAsync(createSubResult.Value, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            var subscription = createSubResult.Value;
            return _mapper.Map<CreateMemberWithSubscriptionCommandResponse>(subscription);
        }

    }
}
