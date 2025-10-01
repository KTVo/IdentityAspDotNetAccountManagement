using System.Security.Claims;
using DotNet9EFAPI.MVCS.Models._DB.Identity;
using DotNet9EFAPI.MVCS.Models.CRUD.Identity;
using DotNet9EFAPI.MVCS.Models.DummyData;
using DotNet9EFAPI.MVCS.Models.JWT;

namespace DotNet9EFAPI.MVCS.Services._DB.JWT;

public interface ITokenProvider
{
    TokenResponse? Create(User user);
    TokenValidateResponse ValidateToken(UserAuthenticationRequest userAuthenticationRequest);
    Task<ToDoResponse?> AuthorizationTestAsync(UserAuthenticationRequest userAuthenticationRequest);

}
