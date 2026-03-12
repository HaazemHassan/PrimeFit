using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.BranchReviews.Commands.AddBranchReview
{
    internal class AddBranchReviewCommandMapping : Profile
    {
        public AddBranchReviewCommandMapping()
        {
            CreateMap<BranchReview, AddBranchReviewCommandResponse>()
              .ForMember(dest => dest.ReviewedAt,
                  opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
