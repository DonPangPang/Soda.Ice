namespace Soda.Ice.WebApi.Helpers;

public class VPagedList<T> : List<T>
{
    public int CurrentPage { get; private set; } = 1;
    public int TotalPages { get; private set; } = 1;
    public int PageSize { get; private set; } = 10;
    public int TotalCount { get; private set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}