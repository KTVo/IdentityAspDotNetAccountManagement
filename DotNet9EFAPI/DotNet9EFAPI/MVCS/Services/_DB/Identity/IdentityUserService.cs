using System.Diagnostics;
using DotNet9EFAPI.MVCS.Models._DB.Identity;
using DotNet9EFAPI.MVCS.Models.CRUD.Identity;
using DotNet9EFAPI.MVCS.Models.JWT;
using DotNet9EFAPI.MVCS.Services._DB.Identity;
using DotNet9EFAPI.MVCS.Services._DB.JWT;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.AspNetCore.Identity;

namespace DotNet9EFAPI.MVCS.Services.Identity;

public sealed class IdentityUserService : IIdentityUserService
{
    // IDENTITY THAT OBJECT FOR MANAGING USER
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenProvider _tokenProvider;

    // CONSTRUCTOR
    public IdentityUserService(UserManager<User> userManager, SignInManager<User> signInManager, ITokenProvider tokenProvider)
    {
        // ASSIGNS DEPENDENCY INJECTED IDENTITY INSTANCES TO CLASS VARIABLE
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
    }

    /// <summary>
    /// CREATES USER ON THE DATABASE USING IDENTITY
    /// </summary>
    /// <param name="user"></param>
    /// <returns>A BOOLEAN ON CREATE USER STATUS</returns>
    public async Task<bool> CreateUserAsync(User user)
    {
        try
        {
#pragma warning disable CS8604 // Possible null reference argument.
            IdentityResult createUserResult = await _userManager.CreateAsync(user, user.PasswordHash);
#pragma warning restore CS8604 // Possible null reference argument.

            return createUserResult.Succeeded;
        }
        catch (Exception e)
        {
            Debug.WriteLine(AppMessages.CreatingUserFailed + " -> " + e.Message);
            return false;
        }

    }
    
    /// <summary>
    /// CREATES USER ON THE DATABASE USING IDENTITY
    /// </summary>
    /// <param name="user"></param>
    /// <returns>A BOOLEAN ON CREATE USER STATUS</returns>
    public async Task<TokenResponse?> LogInUserAsync(string username, string password)
    {
        try
        {
            // FIND USER FROM DB
            User? user = await _userManager.FindByNameAsync(username);
            if (user == null) { return null; }

            // USES IDENTITY TO VALIDATE CREDENTIALS ON DB
            bool valid = await _userManager.CheckPasswordAsync(user, password);

            if (valid == false) { return null; }
            

            TokenResponse? token = _tokenProvider.Create(user);
            return token;
            

           
        }
        catch (Exception e)
        {
            Debug.WriteLine(AppMessages.LoggingInUserFailed + " -> " + e.Message);
            return null;
        }

    }

    public async Task<UpdateAccountDetailsResponse> UpdateUserPasswordAsync(ChangePasswordRequest changePasswordRequest)
    {
        try
        {
            if (changePasswordRequest.Email == null) { return new UpdateAccountDetailsResponse {IsSuccessful = false, Message = AppMessages.NullParameter}; }
            if (changePasswordRequest.Username == null) { return new UpdateAccountDetailsResponse {IsSuccessful = false, Message = AppMessages.NullParameter}; }
            if (changePasswordRequest.NewPassword == null) { return new UpdateAccountDetailsResponse {IsSuccessful = false, Message = AppMessages.NullParameter}; }
            if (changePasswordRequest.OldPassword == null) { return new UpdateAccountDetailsResponse {IsSuccessful = false, Message = AppMessages.NullParameter}; }
            
            // FIND USER FROM DB
            User? user = await _userManager.FindByNameAsync(changePasswordRequest.Email);
            
            if (user == null) { user = await _userManager.FindByNameAsync(changePasswordRequest.Username); }
            
            if (user == null) 
                return new UpdateAccountDetailsResponse
                {
                    Username = null,
                    Email = null,
                    IsSuccessful = false,
                    Message = AppMessages.CannotFindUserToUpdatePasswordFailed
                    
                }; 
            
            // CHANGE USER'S PASSWORD
            IdentityResult changedUserResult = await _userManager.ChangePasswordAsync(
                user: user,
                currentPassword: changePasswordRequest.OldPassword,
                newPassword: changePasswordRequest.NewPassword
            );
            
            if (changedUserResult.Succeeded == false)
            {
                return new UpdateAccountDetailsResponse
                {
                    Username = changePasswordRequest.Username,
                    Email = changePasswordRequest.Email,
                    IsSuccessful = false,
                    Message = AppMessages.UpdateUserPasswordFailed
                };
            }
            return new UpdateAccountDetailsResponse
            {
                Username = user.UserName,
                Email = user.Email,
                IsSuccessful = true,
                Message = AppMessages.UpdateUserPasswordSuccess
                
            };
        }
        catch (Exception ex)
        {
            return new UpdateAccountDetailsResponse
            {
                Username = changePasswordRequest.Username,
                Email = changePasswordRequest.Email,
                IsSuccessful = false,
                Message = ex.Message
                
            };
        }
    }
}
