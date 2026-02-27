namespace PrimeFit.Application.Common.Pagination;

public class PaginatedResult<T>
{



    public List<T> Data { get; set; }

    public PaginatedResult(List<T> data)
    {
        Data = data;
    }

    internal PaginatedResult(List<T> data, int count = 0, int page = 1, int pageSize = PaginationConstants.DefaultPageSize)
    {
        if (page < 1)
        {
            page = 1;

        }

        if (pageSize < PaginationConstants.MinimumPageSize)
        {
            pageSize = PaginationConstants.DefaultPageSize;

        }

        if (pageSize > PaginationConstants.MaximumPageSize)
        {
            pageSize = PaginationConstants.MaximumPageSize;
        }

        Data = data;
        CurrentPage = page;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
    }



    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public int TotalCount { get; set; }

    public object? Meta { get; set; }

    public int PageSize { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;

}
