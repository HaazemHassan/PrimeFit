using AutoMapper;
using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Branches.Commands.CreateBranch
{
    public class CreateBranchCommandHandler : IRequestHandler<CreateBranchCommand, ErrorOr<CreateBranchCommandResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly ICurrentUserService _currentUserService;

        public CreateBranchCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhoneNumberService phoneNumberService, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _phoneNumberService = phoneNumberService;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<CreateBranchCommandResponse>> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
        {
            int curUserId = _currentUserService.UserId!.Value;

            var phoneNumberNormalized = _phoneNumberService.Normalize(request.PhoneNumber!);

            var createBranchResult = Branch.Create(curUserId, request.Name, request.Email, phoneNumberNormalized, request.BranchType);
            if (createBranchResult.IsError)
            {
                return createBranchResult.Errors;

            }

            var branch = createBranchResult.Value;

            await _unitOfWork.Branches.AddAsync(branch, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateBranchCommandResponse(branch.Id);
        }
    }
}
