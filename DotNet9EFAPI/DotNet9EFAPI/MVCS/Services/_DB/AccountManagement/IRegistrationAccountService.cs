using DotNet9EFAPI.MVCS.Models._DB.AccountManagement;

namespace DotNet9EFAPI.MVCS.Services._DB.AccountManagement;

public interface IRegistrationAccountService
{
    Task<bool> CreateUserAsync(User user);
}