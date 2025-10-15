using DotNet9EFAPI.MVCS.Models._DB.Identity;
using DotNet9EFAPI.MVCS.Models.CRUD.Identity;
using DotNet9EFAPI.MVCS.Models.JWT;

namespace DotNet9EFAPI.MVCS.Services._DB.Identity;

public interface IIdentityUserService
{
    Task<bool> CreateUserAsync(User user);
    Task<TokenResponse?> LogInUserAsync(string username, string password);
    Task<UpdateAccountDetailsResponse> UpdateUserEmailAsync(ChangeEmailRequest changeEmailRequest);
    Task<UpdateAccountDetailsResponse> UpdatePhoneNumberAsync(ChangePhoneNumberRequest changePhoneRequest);
    Task<UpdateAccountDetailsResponse> UpdateUserPasswordAsync(ChangePasswordRequest changePasswordRequest);
    Task<UpdateAccountDetailsResponse> UpdateUserAsync(ChangeUserRequest model);
    Task<InitiatePasswordResetResponse> RequestPasswordReset(InitiatePasswordResetRequest model);
}
