using DotNet9EFAPI.MVCS.Models._DB.AccountManagement;
using DotNet9EFAPI.MVCS.Models.CRUD.AccountManagement;
using DotNet9EFAPI.MVCS.Models.JWT;
using DotNet9EFAPI.MVCS.Services._DB.JWT;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.AspNetCore.Identity;

namespace DotNet9EFAPI.MVCS.Services._DB.AccountManagement;

public sealed class UpdateAccountDetailsService : IUpdateAccountDetailsService
{
    // IDENTITY THAT OBJECT FOR MANAGING USER
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;
 

    // CONSTRUCTOR
    public UpdateAccountDetailsService(
        UserManager<User> userManager,
        ITokenProvider tokenProvider)
    {
        // ASSIGNS DEPENDENCY INJECTED IDENTITY INSTANCES TO CLASS VARIABLE
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
    }

    /// <summary>
    /// UPDATES THE USER'S EMAIL ON THE DATABASE
    /// </summary>
    /// <param name="changeEmailRequest"></param>
    /// <returns></returns>
    public async Task<UpdateAccountDetailsResponse> UpdateUserEmailAsync(ChangeEmailRequest changeEmailRequest)
    {
        // NULL CHECKS
        if (changeEmailRequest.JWTToken == null)
        {
            return new UpdateAccountDetailsResponse { IsSuccessful = false, Message = AppMessages.NullParameter };
        }

        if (changeEmailRequest.NewEmail == null)
        {
            return new UpdateAccountDetailsResponse { IsSuccessful = false, Message = AppMessages.NullParameter };
        }

        // VALIDATES IF GIVEN JWT IS ACTIVE
        TokenValidateResponse tokenValidateResponse = _tokenProvider.ValidateToken(new UserAuthenticationRequest()
        {
            JWTToken = changeEmailRequest.JWTToken
        });


        // NULL CHECKS
        if (tokenValidateResponse.IsSuccessful == false)
        {
            return new UpdateAccountDetailsResponse { IsSuccessful = false, Message = tokenValidateResponse.Message };
        }

        if (string.IsNullOrEmpty(tokenValidateResponse.RetrievedEmail))
        {
            return new UpdateAccountDetailsResponse { IsSuccessful = false, Message = AppMessages.NullParameter };
        }

        if (string.IsNullOrEmpty(tokenValidateResponse.RetrievedUsername))
        {
            return new UpdateAccountDetailsResponse { IsSuccessful = false, Message = AppMessages.NullParameter };
        }

        // SEARCHES USER ON DATABASE
        User? user = await _userManager.FindByNameAsync(tokenValidateResponse.RetrievedUsername) ??
                     await _userManager.FindByEmailAsync(tokenValidateResponse.RetrievedEmail);

        if (user == null)
        {
            return new UpdateAccountDetailsResponse()
                { IsSuccessful = false, Message = AppMessages.CannotFindUserToUpdateEmailFailed };
        }

        string? tokenToChangeEmail =
            await _userManager.GenerateChangeEmailTokenAsync(user: user, newEmail: changeEmailRequest.NewEmail);

        if (string.IsNullOrEmpty(tokenToChangeEmail) == true)
        {
            return new UpdateAccountDetailsResponse
                { IsSuccessful = false, Message = AppMessages.GenerateTokenToChangeEmailFailed };
        }

        IdentityResult response = await _userManager.ChangeEmailAsync(user: user, newEmail: changeEmailRequest.NewEmail,
            token: tokenToChangeEmail);

        if (response.Succeeded == false)
        {
            return new UpdateAccountDetailsResponse { IsSuccessful = false, Message = AppMessages.EmailChangeFailed };
        }

        return new UpdateAccountDetailsResponse { IsSuccessful = true, Message = AppMessages.EmailChangeSuccess };
    }

    /// <summary>
    /// UPDATES THE USER'S PHONENUMBER ON THE DATABASE
    /// </summary>
    /// <param name="changePhoneRequest"></param>
    /// <returns></returns>
    public async Task<UpdateAccountDetailsResponse> UpdatePhoneNumberAsync(ChangePhoneNumberRequest changePhoneRequest)
    {
        try
        {
            // NULL CHECKS
            if (changePhoneRequest.JWTToken == null)
            {
                return new UpdateAccountDetailsResponse { IsSuccessful = false, Message = AppMessages.NullParameter };
            }

            if (changePhoneRequest.NewPhoneNumber == null)
            {
                return new UpdateAccountDetailsResponse { IsSuccessful = false, Message = AppMessages.NullParameter };
            }

            // VALIDATES IF GIVEN JWT IS ACTIVE
            TokenValidateResponse tokenValidateResponse = _tokenProvider.ValidateToken(new UserAuthenticationRequest()
            {
                JWTToken = changePhoneRequest.JWTToken
            });

            // SEARCHES USER ON DATABASE
            if (tokenValidateResponse.IsSuccessful == false)
            {
                return new UpdateAccountDetailsResponse
                    { IsSuccessful = false, Message = tokenValidateResponse.Message };
            }

            if (string.IsNullOrEmpty(tokenValidateResponse.RetrievedEmail))
            {
                return new UpdateAccountDetailsResponse { IsSuccessful = false, Message = AppMessages.NullParameter };
            }

            if (string.IsNullOrEmpty(tokenValidateResponse.RetrievedUsername))
            {
                return new UpdateAccountDetailsResponse { IsSuccessful = false, Message = AppMessages.NullParameter };
            }

            User? user = await _userManager.FindByNameAsync(tokenValidateResponse.RetrievedEmail);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(tokenValidateResponse.RetrievedUsername);
            }

            if (user == null)
            {
                return new UpdateAccountDetailsResponse()
                    { IsSuccessful = false, Message = AppMessages.CannotFindUserToUpdateEmailFailed };
            }

            string? tokenToChangePhoneNumber =
                await _userManager.GenerateChangePhoneNumberTokenAsync(user: user,
                    phoneNumber: changePhoneRequest.NewPhoneNumber);

            if (string.IsNullOrEmpty(tokenToChangePhoneNumber) == true)
            {
                return new UpdateAccountDetailsResponse
                    { IsSuccessful = false, Message = AppMessages.GenerateTokenToChangePhoneFailed };
            }

            IdentityResult response = await _userManager.ChangePhoneNumberAsync(user: user,
                phoneNumber: changePhoneRequest.NewPhoneNumber, token: tokenToChangePhoneNumber);

            if (response.Succeeded == false)
            {
                return new UpdateAccountDetailsResponse
                    { IsSuccessful = false, Message = AppMessages.PhoneNumberChangeFailed };
            }

            return new UpdateAccountDetailsResponse
                { IsSuccessful = true, Message = AppMessages.PhoneNumberChangeSuccess };
        }
        catch (Exception ex)
        {
            return new UpdateAccountDetailsResponse { IsSuccessful = true, Message = ex.Message };
        }

    }

    /// <summary>
    /// UPDATES THE USER'S ON THE DATABASE
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<UpdateAccountDetailsResponse> UpdateUserAsync(ChangeUserRequest model)
    {
        try
        {
            // NULL CHECKS
            if (string.IsNullOrEmpty(model.NewUserName))
            {
                return new UpdateAccountDetailsResponse { IsSuccessful = false, Message = AppMessages.NullParameter };
            }

            // VALIDATES IF GIVEN JWT IS ACTIVE
            TokenValidateResponse tokenValidateResponse = _tokenProvider.ValidateToken(new UserAuthenticationRequest()
            {
                JWTToken = model.JWTToken
            });

            // SEARCHES USER ON DATABASE
            if (tokenValidateResponse.IsSuccessful == false)
            {
                return new UpdateAccountDetailsResponse
                    { IsSuccessful = false, Message = tokenValidateResponse.Message };
            }

            if (string.IsNullOrEmpty(tokenValidateResponse.RetrievedEmail))
            {
                return new UpdateAccountDetailsResponse { IsSuccessful = false, Message = AppMessages.NullParameter };
            }

            if (string.IsNullOrEmpty(tokenValidateResponse.RetrievedUsername))
            {
                return new UpdateAccountDetailsResponse { IsSuccessful = false, Message = AppMessages.NullParameter };
            }

            User? user = await _userManager.FindByNameAsync(tokenValidateResponse.RetrievedEmail);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(tokenValidateResponse.RetrievedUsername);
            }

            if (user == null)
            {
                return new UpdateAccountDetailsResponse()
                    { IsSuccessful = false, Message = AppMessages.CannotFindUserToUpdateEmailFailed };
            }

            if (model.UpdateUserName == true)
            {
                user.UserName = model.NewUserName;
            }

            IdentityResult response = await _userManager.UpdateAsync(user: user);

            if (response.Succeeded == false)
            {
                return new UpdateAccountDetailsResponse
                    { IsSuccessful = false, Message = AppMessages.UserChangeFailed };
            }

            return new UpdateAccountDetailsResponse { IsSuccessful = true, Message = AppMessages.UserChangeSuccess };
        }
        catch (Exception ex)
        {
            return new UpdateAccountDetailsResponse { IsSuccessful = true, Message = ex.Message };
        }

    }
}
