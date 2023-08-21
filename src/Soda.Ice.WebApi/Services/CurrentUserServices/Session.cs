using Microsoft.EntityFrameworkCore;
using Soda.Ice.Domain;
using Soda.Ice.WebApi.Auth;
using Soda.Ice.WebApi.UnitOfWorks;

namespace Soda.Ice.WebApi.Services.CurrentUserServices;

public class Session
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Guid UserId => Identity.UserId ?? Guid.Empty;

    public Session(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CustomIdentity Identity => _httpContextAccessor?.HttpContext?.User?.Identity as CustomIdentity ?? new CustomIdentity();

    public User? User { get; set; }
}