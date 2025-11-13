using DotNet9EFAPI.MVCS.Models.CRUD.AccountManagement;

namespace DotNet9EFAPI.MVCS.Services._DB.AccountManagement;

public interface IPasswordResetService
{
    Task<UpdateAccountDetailsResponse> UpdateUserPasswordAsync(ChangePasswordRequest changePasswordRequest);
    Task<InitiatePasswordResetResponse> RequestPasswordResetAsync(InitiatePasswordResetRequest model);
    Task<InitiatePasswordResetResponse> ResetPasswordAsync(RecoverPasswordRequest model);
    
}