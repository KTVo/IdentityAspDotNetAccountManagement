using System.Diagnostics;
using DotNet9EFAPI.MVCS.Models._DB.Identity;
using DotNet9EFAPI.MVCS.Models.CRUD.Identity;
using DotNet9EFAPI.MVCS.Services._DB.Identity;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace DotNet9EFAPI.MVCS.Services.Identity;

public class IdentityUserService : IIdentityUserService
{
    // IDENTITY THAT OBJECT FOR MANAGING USER
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    // CONSTRUCTOR
    public IdentityUserService(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        // ASSIGNS DEPENDENCY INJECTED IDENTITY INSTANCES TO CLASS VARIABLE
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
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
    public async Task<bool> LogInUserAsync(string username, string password)
    {
        try
        {
#pragma warning disable CS8604 // Possible null reference argument.
            SignInResult loginUserResult = await _signInManager.PasswordSignInAsync(
                username, password, isPersistent: false, lockoutOnFailure: false);
#pragma warning restore CS8604 // Possible null reference argument.

            return loginUserResult.Succeeded;
        }
        catch (Exception e)
        {
            Debug.WriteLine(AppMessages.LoggingInUserFailed + " -> " + e.Message);
            return false;
        }

    }
}
