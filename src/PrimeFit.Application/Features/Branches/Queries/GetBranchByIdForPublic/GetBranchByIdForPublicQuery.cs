using ErrorOr;
using MediatR;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchByIdForPublic
{

    public class GetBranchByIdForPublicQuery : IRequest<ErrorOr<GetBranchByIdForPublicQueryResponse>>
    {

        public int BranchId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public GetBranchByIdForPublicQuery()
        {
        }

        public GetBranchByIdForPublicQuery(int branchId)
        {
            BranchId = branchId;
        }


        public GetBranchByIdForPublicQuery(int branchId, double? latitute, double? longitude)
        {
            BranchId = branchId;
            Latitude = latitute;
            Longitude = longitude;
        }


    }
}
