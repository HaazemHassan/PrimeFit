using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.BranchReviews.Commands.UpdateMyBranchReview
{
    internal class UpdateMyBranchReviewCommandMapping : Profile
    {
        public UpdateMyBranchReviewCommandMapping()
        {
            CreateMap<BranchReview, UpdateMyBranchReviewCommandResponse>()
                .ForMember(dest => dest.ReviewedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
