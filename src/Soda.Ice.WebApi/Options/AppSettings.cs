using Microsoft.Extensions.Options;
using Soda.Ice.WebApi.Auth;

namespace Soda.Ice.WebApi.Options;

public class AppSettings : IOptions<AppSettings>
{
    public PermissionRequirement TokenParameter { get; set; } = new();
    public AppSettings Value => this;
}