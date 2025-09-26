using DotNet9EFAPI.MVCS.Models._DB.Identity;
using DotNet9EFAPI.MVCS.Models.CRUD.Identity;
using DotNet9EFAPI.MVCS.Models.JWT;
using DotNet9EFAPI.MVCS.Services._DB.JWT;
using Microsoft.AspNetCore.Identity.Data;

namespace DotNet9EFAPI.MVCS.Services._DB.Identity;

public interface IIdentityUserService
{
    Task<bool> CreateUserAsync(User user);
    Task<TokenResponse?> LogInUserAsync(string username, string password);
}
