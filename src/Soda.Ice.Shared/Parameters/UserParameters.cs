using Soda.Ice.Abstracts;
using Soda.Ice.Common.Attributes;

namespace Soda.Ice.WebApi.Controllers;

public class UserParameters : IceParameters, IPaging, ISorting
{
    [IceCompare(Operation.Contains)]
    public string? Name { get; set; }

    [IceCompare(Operation.Contains)]
    public string? Email { get; set; }

    [IceCompare(Operation.Contains)]
    public string? Account { get; set; }

    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? OrderBy { get; set; }
}