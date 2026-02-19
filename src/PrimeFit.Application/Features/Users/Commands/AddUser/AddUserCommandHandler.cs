using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Contracts.Repositories;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Users.Commands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, ErrorOr<AddUserCommandResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IPhoneNumberService _phoneNumberService;

        public AddUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUserService applicationUserService, IPhoneNumberService phoneNumberService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _applicationUserService = applicationUserService;
            _phoneNumberService = phoneNumberService;
        }

        public async Task<ErrorOr<AddUserCommandResponse>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var phoneNumberNormalized = _phoneNumberService.Normalize(request.PhoneNumber!);

            var domainUser = new DomainUser(request.FirstName, request.LastName, request.Email, phoneNumberNormalized, request.Address);
            var addUserResult = await _applicationUserService.AddUser(domainUser, request.Password, request.UserRole, cancellationToken);

            if (addUserResult.IsError)
                return addUserResult.Errors;

            var response = _mapper.Map<AddUserCommandResponse>(addUserResult.Value);
            return response;
        }
    }
}
