using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Soda.Ice.Domain;
using Soda.Ice.WebApi.Options;
using Soda.Ice.WebApi.Services.CurrentUserServices;
using Soda.Ice.WebApi.UnitOfWorks;
using System.Security.Claims;

namespace Soda.Ice.WebApi.Auth;

/// <summary>
/// 重写Permission
/// </summary>
public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;

    private readonly PermissionRequirement _tokenParameter;

    private Session _session;

    public PermissionHandler(
        IConfiguration config,
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        Session session,
        IOptions<AppSettings> options)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        _tokenParameter = options.Value.TokenParameter;
        _session = session;
    }

    /// <summary>
    /// </summary>
    /// <param name="context">     </param>
    /// <param name="requirement"> </param>
    /// <returns> </returns>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        // 校验 颁发和接收对象
        if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth &&
                                        c.Issuer == _tokenParameter.Issuer))
        {
            await Task.CompletedTask;
        }

        var dateOfBirth = Convert.ToDateTime(context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth &&
                                                                         c.Issuer == _tokenParameter.Issuer)
            ?.Value);

        var accessExpiration = dateOfBirth.AddMinutes(_tokenParameter.AccessExpiration);
        var nowExpiration = DateTime.Now;
        if (accessExpiration < nowExpiration)
        {
            context.Fail();
            await Task.CompletedTask;
            return;
        }

        var id = Guid.Parse(context.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Sid))!.Value);

        var user = await _unitOfWork.Query<User>().Where(x => x.Id == id).FirstOrDefaultAsync();

        if (user is not null)
        {
            _session.User = user;
        }
        else
        {
            context.Fail();
            await Task.CompletedTask;
            return;
        }
        context.Succeed(requirement);
        await Task.CompletedTask;
    }
}