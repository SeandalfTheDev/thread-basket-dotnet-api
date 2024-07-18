namespace ThreadBasket.Application;

public class PagedList<T>(
    IEnumerable<T> data,
    long currentPage,
    long pageSize,
    long pageCount,
    long totalCount,
    bool hasNextPage,
    bool hasPreviousPage)
{
    public IEnumerable<T> Data { get; set; } = data;
    public long CurrentPage { get; set; } = currentPage;
    public long PageSize { get; set; } = pageSize;
    public long PageCount { get; set; } = pageCount;
    public long TotalCount { get; set; } = totalCount;
    public bool HasNextPage { get; set; } = hasNextPage;
    public bool HasPreviousPage { get; set; } = hasPreviousPage;
}