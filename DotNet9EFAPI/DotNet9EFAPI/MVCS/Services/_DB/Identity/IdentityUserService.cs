using System.Diagnostics;
using DotNet9EFAPI.MVCS.Models._DB.Identity;
using DotNet9EFAPI.MVCS.Models.CRUD.Identity;
using DotNet9EFAPI.MVCS.Services._DB.Identity;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.AspNetCore.Identity;

namespace DotNet9EFAPI.MVCS.Services.Identity;

public class IdentityUserService : IIdentityUserService
{
    private readonly UserManager<User> _userManager;
    public IdentityUserService(UserManager<User> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }
    
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
