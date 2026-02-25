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
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ErrorOr<BaseUserResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IPhoneNumberService _phoneNumberService;

        public RegisterUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUserService applicationUserService, IAuthenticationService authenticationService, IPhoneNumberService phoneNumberService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _applicationUserService = applicationUserService;
            _authenticationService = authenticationService;
            _phoneNumberService = phoneNumberService;
        }

        public async Task<ErrorOr<BaseUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var normalizedPhoneNumber = _phoneNumberService.Normalize(request.PhoneNumber!);

            var userToAdd = new DomainUser(request.FirstName, request.LastName, request.Email, normalizedPhoneNumber);
            var addUserResult = await _applicationUserService.AddUser(userToAdd, request.Password, ct: cancellationToken);

            if (addUserResult.IsError)
                return addUserResult.Errors;

            var userResponse = _mapper.Map<BaseUserResponse>(addUserResult.Value);
            userResponse.Role = UserRole.Member;
            return userResponse;
        }
    }
}
