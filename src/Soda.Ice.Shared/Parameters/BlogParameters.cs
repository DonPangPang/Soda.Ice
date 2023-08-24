using Soda.Ice.Abstracts;
using Soda.Ice.Common.Attributes;

namespace Soda.Ice.Shared.Parameters;

public class BlogParameters : IceParameters, IDateRange, IPaging, ISorting
{
    [IceCompare(Operation.Contains)]
    public string? Title { get; set; }

    [IceCompare(Operation.Contains)]
    public string? Descrption { get; set; }

    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; }
}

public class BlogSearchParameters : BlogParameters
{
    public string? SearchKey { get; set; }

    [IceCompare(Operation.Contains, "BlogTags.Id")]
    public Guid TagId { get; set; }

    [IceCompare(Operation.Contains, "BlogGroups.Id")]
    public Guid GroupId { get; set; }
}