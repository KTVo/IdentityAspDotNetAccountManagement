using DotNet9EFAPI.MVCS.Models.JWT;

namespace DotNet9EFAPI.MVCS.Services._DB.AccountManagement;

public interface ILoginAccountService
{
    Task<TokenResponse?> LogInUserAsync(string username, string password);
}