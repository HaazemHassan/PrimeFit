using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Application.Features.Users.Common;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Authentication.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ErrorOr<UserBaseResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly ITotpService _totpService;
        private readonly IOtpService _otpService;
        private readonly IDateTimeProvider _dateTimeProvider;


        public RegisterUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUserService applicationUserService, IAuthenticationService authenticationService, IPhoneNumberService phoneNumberService, ITotpService totpService, IOtpService otpService, IDateTimeProvider dateTimeProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _applicationUserService = applicationUserService;
            _authenticationService = authenticationService;
            _phoneNumberService = phoneNumberService;
            _totpService = totpService;
            _otpService = otpService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ErrorOr<UserBaseResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var normalizedPhone = _phoneNumberService.Normalize(request.PhoneNumber!);

            var existingDomainUser = await _unitOfWork.Users.GetAsync(u => u.Email == request.Email, cancellationToken);

            DomainUser domainUser;

            if (existingDomainUser is not null)
            {
                existingDomainUser.UpdateInfo(request.FirstName, request.LastName, normalizedPhone);
                domainUser = existingDomainUser;
            }
            else
            {
                var phoneExists = await _unitOfWork.Users.AnyAsync(u => u.PhoneNumber == normalizedPhone, cancellationToken);

                if (phoneExists)
                {
                    return Error.Conflict(
                        code: ErrorCodes.User.PhoneAlreadyExists,
                        description: "Phone number already exists");
                }

                var totpSecret = _totpService.GenerateTotpSecret();

                domainUser = new DomainUser(
                    UserType.Customer,
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    normalizedPhone,
                    totpSecret);

            }

            var addUserResult = await _applicationUserService.AddUser(domainUser, request.Password, ct: cancellationToken);

            if (addUserResult.IsError)
            {
                return addUserResult.Errors;

            }

            int appUserId = addUserResult.Value;

            await _authenticationService.CreateEmailConfirmationCode(appUserId, cancellationToken);

            var response = _mapper.Map<UserBaseResponse>(domainUser);
            response.UserType = UserType.Customer;

            return response;
        }
    }
}
