using System.Diagnostics;
using DotNet9EFAPI.MVCS.Models._DB.AccountManagement;
using DotNet9EFAPI.MVCS.Models.Email;
using DotNet9EFAPI.MVCS.Models.JWT;
using DotNet9EFAPI.MVCS.Services._DB.JWT;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DotNet9EFAPI.MVCS.Services._DB.AccountManagement;

public class LoginAccountService : ILoginAccountService
{
    // IDENTITY THAT OBJECT FOR MANAGING USER
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly EmailSettings _emailSettings;

    // CONSTRUCTOR
    public LoginAccountService(
        UserManager<User> userManager, 
        SignInManager<User> signInManager, 
        ITokenProvider tokenProvider,
        IOptions<EmailSettings> emailSettings)
    {
        // ASSIGNS DEPENDENCY INJECTED IDENTITY INSTANCES TO CLASS VARIABLE
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        _emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings));
    }
    
    /// <summary>
    /// LOGINS TO A VALID ACCOUNT
    /// </summary>
    /// <param name="user"></param>
    /// <returns>A BOOLEAN ON CREATE USER STATUS</returns>
    public async Task<TokenResponse?> LogInUserAsync(string username, string password)
    {
        try
        {
            if (string.IsNullOrEmpty(username) == null) { return new() { IsSuccessful = false, Message = AppMessages.NullParameter }; }
            if (string.IsNullOrEmpty(password) == null) { return new() { IsSuccessful = false, Message = AppMessages.NullParameter }; }
            
            // FIND USER FROM DB
            User? user = await _userManager.FindByNameAsync(username) ?? await _userManager.FindByEmailAsync(username);

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
            return new() { IsSuccessful = false, Message = e.Message };
        }

    }
}