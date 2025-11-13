using DotNet9EFAPI.MVCS.Models.CRUD.AccountManagement;

namespace DotNet9EFAPI.MVCS.Services._DB.AccountManagement;

public interface IUpdateAccountDetailsService
{
    
    Task<UpdateAccountDetailsResponse> UpdateUserEmailAsync(ChangeEmailRequest changeEmailRequest);
    Task<UpdateAccountDetailsResponse> UpdatePhoneNumberAsync(ChangePhoneNumberRequest changePhoneRequest);
    Task<UpdateAccountDetailsResponse> UpdateUserAsync(ChangeUserRequest model);
}
