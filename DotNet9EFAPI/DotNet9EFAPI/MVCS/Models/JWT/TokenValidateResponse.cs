using System.Security.Claims;
using DotNet9EFAPI.MVCS.Models._base;
using Microsoft.IdentityModel.Tokens;

namespace DotNet9EFAPI.MVCS.Models.JWT;

public class TokenValidateResponse : BaseResponse
{
    public SecurityToken? ValidatedToken { get; set; }
    public ClaimsPrincipal? ClaimsPrincipal { get; set; }
}
