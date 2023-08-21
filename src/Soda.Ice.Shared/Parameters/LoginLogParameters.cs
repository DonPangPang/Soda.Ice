using Soda.Ice.Abstracts;

namespace Soda.Ice.WebApi.Controllers;

public class LoginLogParameters : IceParameters, IPaging, ISorting
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? OrderBy { get; set; }
}