using Ardalis.Specification;
using PrimeFit.Application.Common.Pagination;

namespace PrimeFit.Application.Specifications
{
    public static class SpecificationBuilderExtensions
    {
        public static ISpecificationBuilder<T> Paginate<T>(
            this ISpecificationBuilder<T> builder, int pageNumber, int pageSize)
        {

            if (pageNumber <= 0)
                pageNumber = 1;

            if (pageSize < PaginationConstants.MinimumPageSize)
            {
                pageSize = PaginationConstants.DefaultPageSize;

            }
            else if (pageSize > PaginationConstants.MaximumPageSize)
            {
                pageSize = PaginationConstants.MaximumPageSize;

            }

            return builder.Skip((pageNumber - 1) * pageSize)
                          .Take(pageSize);
        }
    }
}