//using ErrorOr;
//using MediatR;
//using PrimeFit.Application.Contracts.Api;
//using PrimeFit.Application.Specifications.Branches;
//using PrimeFit.Domain.Common.Constants;
//using PrimeFit.Domain.Repositories;

//namespace PrimeFit.Application.Features.Branches.Queries.GetBranchById
//{
//    public class GetBranchByIdQueryHandler : IRequestHandler<GetBranchByIdQuery, ErrorOr<GetBranchByIdQueryResponse>>
//    {

//        private readonly IUnitOfWork _unitOfWork;
//        private readonly ICurrentUserService _currentUserService;
//        public GetBranchByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
//        {
//            _unitOfWork = unitOfWork;
//            _currentUserService = currentUserService;
//        }



//        public async Task<ErrorOr<GetBranchByIdQueryResponse>> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
//        {
//            int userId = _currentUserService.UserId!.Value;
//            var spec = new BranchForOwnerSpec(request.BranchId, userId);
//            var branch = _unitOfWork.Branches.FirstOrDefaultAsync(spec, cancellationToken);

//            if (branchResponse is null)
//            {
//                return Error.NotFound(
//                    ErrorCodes.Branch.BranchNotFound,
//                    "Branch with the given id was not found or you don't have access to it");
//            }

//            b




//        }
//    }
//}
