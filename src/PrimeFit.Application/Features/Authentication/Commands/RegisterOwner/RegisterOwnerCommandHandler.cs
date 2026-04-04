using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Application.Features.Users.Common;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Authentication.Commands.RegisterOwner
{
    public class RegisterOwnerCommandHandler : IRequestHandler<RegisterOwnerCommand, ErrorOr<UserBaseResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterOwnerCommandHandler(IMapper mapper, IApplicationUserService applicationUserService, IEmailVerificationService emailVerificationService, IPhoneNumberService phoneNumberService, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _applicationUserService = applicationUserService;
            _emailVerificationService = emailVerificationService;
            _phoneNumberService = phoneNumberService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<UserBaseResponse>> Handle(RegisterOwnerCommand request, CancellationToken cancellationToken)
        {
            string? normalizedPhoneNumber = null;
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                normalizedPhoneNumber = _phoneNumberService.Normalize(request.PhoneNumber);
            }

            var isEmailUsed = await _unitOfWork.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);

            if (isEmailUsed)
            {
                return Error.Conflict(
                   code: ErrorCodes.User.EmailAlreadyExists,
                   description: "Email already exists");
            }

            if (!string.IsNullOrWhiteSpace(normalizedPhoneNumber))
            {
                var isPhoneUsed = await _unitOfWork.Users.AnyAsync(u => u.PhoneNumber == normalizedPhoneNumber, cancellationToken);

                if (isPhoneUsed)
                {
                    return Error.Conflict(
                       code: ErrorCodes.User.PhoneAlreadyExists,
                       description: "Phone already exists");
                }
            }

            var userToAdd = new DomainUser(
                UserType.PartnerAdmin,
                request.FirstName,
                request.LastName,
                request.Email,
                normalizedPhoneNumber);


            var addUserResult = await _applicationUserService.AddUser(
                userToAdd,
                request.Password,
                null,
                ct: cancellationToken);

            if (addUserResult.IsError)
            {
                return addUserResult.Errors;

            }

            var createCodeResult = await _emailVerificationService.CreateEmailConfirmationCode(userToAdd.Id, cancellationToken);
            if (createCodeResult.IsError)
            {
                return createCodeResult.Errors;
            }

            await _emailVerificationService.SendConfirmationEmailAsync(userToAdd, createCodeResult.Value);
            var userResponse = _mapper.Map<UserBaseResponse>(userToAdd);
            userResponse.UserType = UserType.PartnerAdmin;
            return userResponse;
        }
    }
}
