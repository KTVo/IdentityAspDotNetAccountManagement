using System.Security.Claims;
using DotNet9EFAPI.MVCS.Models._DB.Identity;
using DotNet9EFAPI.MVCS.Models.CRUD.Identity;

namespace DotNet9EFAPI.MVCS.Services._DB.JWT;

public interface ITokenProvider
{
    string? Create(User user);
    string? TestToken(UserAuthenticationRequest userAuthenticationRequest);

}
