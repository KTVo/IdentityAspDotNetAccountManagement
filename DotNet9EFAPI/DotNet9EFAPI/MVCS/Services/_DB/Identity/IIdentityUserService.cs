using DotNet9EFAPI.MVCS.Models._DB.Identity;

namespace DotNet9EFAPI.MVCS.Services._DB.Identity;

public interface IIdentityUserService
{
    Task<bool> CreateUserAsync(User user);
}
