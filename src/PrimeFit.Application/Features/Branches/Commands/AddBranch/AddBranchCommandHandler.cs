using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Features.Branches.Commands.AddBranch;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Branches.Commands.AddBranchBussinessDetails
{
    public class AddBranchCommandHandler : IRequestHandler<AddBranchCommand, ErrorOr<AddBranchCommandResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly ICurrentUserService _currentUserService;

        public AddBranchCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhoneNumberService phoneNumberService, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _phoneNumberService = phoneNumberService;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<AddBranchCommandResponse>> Handle(AddBranchCommand request, CancellationToken cancellationToken)
        {
            var curUserId = _currentUserService.UserId;

            var phoneNumberNormalized = _phoneNumberService.Normalize(request.PhoneNumber!);

            var branch = new Branch(curUserId!.Value);
            branch.SetBussinessDetails(request.Name, request.Email, phoneNumberNormalized, request.BranchType);

            await _unitOfWork.Branches.AddAsync(branch, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AddBranchCommandResponse(branch.Id);
        }
    }
}
