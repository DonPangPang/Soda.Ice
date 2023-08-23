using Soda.Ice.Abstracts;
using Soda.Ice.Common.Attributes;

namespace Soda.Ice.Shared.Parameters;

public class BlogParameters : IceParameters, IDateRange, IPaging, ISorting
{
    [IceCompare(Operation.Contains)]
    public string? Title { get; set; }

    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; }
}