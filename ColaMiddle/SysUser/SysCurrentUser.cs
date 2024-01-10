using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Cola.Core.Models.ColaJwt;
using Cola.CoreUtils.Extensions;
using Microsoft.AspNetCore.Http;
using SqlSugar.Extensions;

namespace Cola.ColaMiddleware.ColaMiddle.SysUser;

public class SysCurrentUser : ISysCurrentUser
{
    private readonly IHttpContextAccessor _accessor;

    public SysCurrentUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public TokenUserInfo GetCurrentUserInfo()
    {
        return new TokenUserInfo()
        {
            CurrentUserId = GetClaimValueByType(JwtRegisteredClaimNames.Jti)!.FirstOrDefault()!,
            CurrentLoginName = GetClaimValueByType(JwtRegisteredClaimNames.Sub)!.FirstOrDefault()!,
            CurrentUserName = _accessor.HttpContext!.User.Identity!.Name!,
        };
    }
    
    public List<string> GetUserInfoFromToken(string claimType)
    {

        var jwtHandler = new JwtSecurityTokenHandler();
        if (!string.IsNullOrEmpty(GetToken()))
        {
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(GetToken());

            return (from item in jwtToken.Claims
                where item.Type == claimType
                select item.Value).ToList();
        }
        else
        {
            return new List<string>() { };
        }
    }
    
    public List<string>? GetClaimValueByType(string claimType)
    {
        var result = GetClaimsIdentity();
        if (result == null)
        {
            return null;
        }

        return (from item in result
            where item.Type == claimType
            select item.Value).ToList();
    }

    private IEnumerable<Claim>? GetClaimsIdentity()
    {
        return _accessor.HttpContext?.User?.Claims;
    }

    private string? GetToken()
    {
        return _accessor.HttpContext?.Request.Headers["Authorization"].ObjToString().Replace("Bearer ", "");
    }

    
}