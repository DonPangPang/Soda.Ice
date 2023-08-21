using Soda.Ice.Common.Extensions;
using Soda.Ice.Shared.Enums;
using System.Security.Claims;

namespace Soda.Ice.WebApi.Auth
{
    public class CustomIdentity : ClaimsIdentity
    {
        private string? GetClaimValue(string claimType, string? defaultValue = null)
        {
            var val = Claims?.FirstOrDefault(e => e.Type == claimType)?.Value;
            return string.IsNullOrWhiteSpace(val) ? defaultValue : val;
        }

        private void SetClaimValue(string claimType, string? value, string? defaultValue = null)
        {
            var claim = Claims?.FirstOrDefault(e => e.Type == claimType);
            if (claim != null)
            {
                RemoveClaim(claim);
            }
            AddClaim(new Claim(claimType, value ?? defaultValue ?? string.Empty));
        }

        public Guid? UserId
        {
            get
            {
                var userId = GetClaimValue("UserId");
                Guid.TryParse(userId, out var id);
                return id;
            }
            set => SetClaimValue("UserId", value.ToString());
        }
    }
}