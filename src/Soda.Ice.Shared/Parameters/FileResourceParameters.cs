using Soda.Ice.Abstracts;
using Soda.Ice.Common.Attributes;

namespace Soda.Ice.WebApi.Controllers;

public class FileResourceParameters : IceParameters, IPaging, ISorting
{
    [IceCompare(Operation.Contains)]
    public string? Name { get; set; }

    [IceCompare(Operation.Equal)]
    public Guid? BlogId { get; set; }

    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? OrderBy { get; set; }
}