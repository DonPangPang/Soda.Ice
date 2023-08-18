using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Soda.AutoMapper;
using Soda.Ice.Domain;
using Soda.Ice.Shared.ViewModels;
using Soda.Ice.WebApi.Auth;
using Soda.Ice.WebApi.Options;
using Soda.Ice.WebApi.Services.CurrentUserServices;
using Soda.Ice.WebApi.UnitOfWorks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Soda.Ice.WebApi.Controllers;

/// <summary>
/// 注册用户信息
/// </summary>
public class AuthorizationController : ApiControllerBase
{
    private readonly Session _session;
    private readonly IUnitOfWork _unitOfWork;
    private PermissionRequirement _tokenParameter;

    /// <summary>
    /// </summary>
    /// <param name="weComServices"> </param>
    /// <param name="session">       </param>
    /// <param name="serviceGen">    </param>
    /// <param name="configuration"> </param>
    public AuthorizationController(
        Session session,
        IConfiguration configuration,
        IUnitOfWork unitOfWork,
        IOptions<AppSettings> options)
    {
        _session = session;
        _unitOfWork = unitOfWork;
        _tokenParameter = options.Value.TokenParameter;
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="request"> </param>
    /// <returns> </returns>
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] VLogin request)
    {
        if (string.IsNullOrEmpty(request.Account) || string.IsNullOrEmpty(request.Password))
            return Fail("Invalid Request");

        var user = await _unitOfWork.Query<User>().Where(x => x.Account.Equals(request.UseAccountrName)).FirstOrDefaultAsync();
        if (user is null)
        {
            return Fail("账号不存在");
        }
        if (user!.Password != request.Password)
        {
            return Fail("密码错误");
        }

        //生成Token和RefreshToken
        var token = GenUserToken(user.Id, request.Account, user.Role.ToString());
        var refreshToken = "123456";

        return Success(new VToken { Token = token, RefreshToken = refreshToken, User = user.MapTo<VUser>() });
    }

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="dto"> </param>
    /// <returns> </returns>
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] VRegisterUser dto)
    {
        if (dto is null) throw new ArgumentNullException(nameof(dto));

        var user = dto.MapTo<User>();

        await _unitOfWork.Db.AddAsync<User>(user);

        var res = await _unitOfWork.CommitAsync();

        if (res) Success("注册成功");

        return Fail("注册失败");
    }

    /// <summary>
    /// 生成Token
    /// </summary>
    /// <param name="id">       </param>
    /// <param name="username"> </param>
    /// <param name="role">     </param>
    /// <returns> </returns>
    private string GenUserToken(Guid id, string username, string role)
    {
        var claims = new[]
        {
                new Claim(ClaimTypes.Sid, id.ToString()),
                new Claim(ClaimTypes.DateOfBirth,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenParameter.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwtToken = new JwtSecurityToken(_tokenParameter.Issuer,
                                            _tokenParameter.Audience,
                                            claims,
                                            expires: DateTime.UtcNow.AddMinutes(_tokenParameter.AccessExpiration),
                                            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return token;
    }
}