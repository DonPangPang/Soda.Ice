using Microsoft.AspNetCore.Authorization;

namespace Soda.Ice.WebApi.Auth;

public class TokenParameter : IAuthorizationRequirement
{
    public string Name { get; set; } = null!;
    public string Secret { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;

    public int AccessExpiration { get; set; }
    public int RefreshExpiration { get; set; }
}