using System.Diagnostics;
using DotNet9EFAPI.MVCS.Models._DB.AccountManagement;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.AspNetCore.Identity;

namespace DotNet9EFAPI.MVCS.Services._DB.AccountManagement;

public sealed class RegisterAccountService : IRegistrationAccountService
{
    // IDENTITY THAT OBJECT FOR MANAGING USER
    private readonly UserManager<User> _userManager;

    // CONSTRUCTOR
    public RegisterAccountService(UserManager<User> userManager)
    {
        // ASSIGNS DEPENDENCY INJECTED IDENTITY INSTANCES TO CLASS VARIABLE
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
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
}