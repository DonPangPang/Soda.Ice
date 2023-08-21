using Soda.Ice.Abstracts;
using Soda.Ice.Common.Attributes;

namespace Soda.Ice.WebApi.Controllers;

public class BlogTagParameters : IceParameters
{
    [IceCompare(Operation.Contains)]
    public string? Name { get; set; }
}