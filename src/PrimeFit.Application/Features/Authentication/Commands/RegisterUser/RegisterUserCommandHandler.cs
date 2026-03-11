using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Application.Features.Users.Common;
using PrimeFit.Application.ServicesContracts.Infrastructure;
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


        public RegisterUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUserService applicationUserService, IAuthenticationService authenticationService, IPhoneNumberService phoneNumberService, ITotpService totpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _applicationUserService = applicationUserService;
            _authenticationService = authenticationService;
            _phoneNumberService = phoneNumberService;
            _totpService = totpService;
        }

        public async Task<ErrorOr<UserBaseResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var normalizedPhoneNumber = _phoneNumberService.Normalize(request.PhoneNumber!);

            var totpSecret = _totpService.GenerateTotpSecret();

            var userToAdd = new DomainUser(UserType.Customer, request.FirstName, request.LastName, request.Email, normalizedPhoneNumber, totpSecret);
            var addUserResult = await _applicationUserService.AddUser(userToAdd, request.Password, ct: cancellationToken);

            if (addUserResult.IsError)
                return addUserResult.Errors;

            var userResponse = _mapper.Map<UserBaseResponse>(addUserResult.Value);
            userResponse.UserType = UserType.Customer;
            return userResponse;
        }
    }
}
