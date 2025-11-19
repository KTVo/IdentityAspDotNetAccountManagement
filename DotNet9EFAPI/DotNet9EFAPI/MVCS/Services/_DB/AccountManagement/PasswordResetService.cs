using DotNet9EFAPI.Helpers.Email;
using DotNet9EFAPI.Helpers.Token.Verify;
using DotNet9EFAPI.MVCS.Models._DB.AccountManagement;
using DotNet9EFAPI.MVCS.Models.CRUD.AccountManagement;
using DotNet9EFAPI.MVCS.Models.Email;
using DotNet9EFAPI.MVCS.Models.JWT;
using DotNet9EFAPI.MVCS.Services._DB.JWT;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DotNet9EFAPI.MVCS.Services._DB.AccountManagement;

public sealed class PasswordResetService : IPasswordResetService
{
    // IDENTITY THAT OBJECT FOR MANAGING USER
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly EmailSettings _emailSettings;
    private readonly TestDBContext? _dbContext;

    // CONSTRUCTOR
    public PasswordResetService(
        UserManager<User> userManager, 
        ITokenProvider tokenProvider,
        IOptions<EmailSettings> emailSettings,
        TestDBContext? dbContext)
    {
        // ASSIGNS DEPENDENCY INJECTED IDENTITY INSTANCES TO CLASS VARIABLE
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        _emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    /// <summary>
    /// ALLOWS USER TO UPDATE THEIR PASSWORD GIVEN THE OLD PASSWORD AND NEW PASSWORD
    /// </summary>
    /// <param name="changePasswordRequest"></param>
    /// <returns></returns>
    public async Task<UpdateAccountDetailsResponse> UpdateUserPasswordAsync(ChangePasswordRequest changePasswordRequest)
    {
        try
        {
            // NULL CHECKS
            if (changePasswordRequest.JWTToken == null) { new UpdateAccountDetailsResponse {IsSuccessful = false, Message = AppMessages.NullParameter}; }
            if (changePasswordRequest.NewPassword == null) { return new UpdateAccountDetailsResponse {IsSuccessful = false, Message = AppMessages.NullParameter}; }
            if (changePasswordRequest.OldPassword == null) { return new UpdateAccountDetailsResponse {IsSuccessful = false, Message = AppMessages.NullParameter}; }
  
            // DECODES JWT TOKEN
            TokenValidateResponse tokenValidateResponse = _tokenProvider.ValidateToken(new UserAuthenticationRequest()
            {
                JWTToken = changePasswordRequest.JWTToken
            });
            
            // NULL CHECKS
            if (tokenValidateResponse.IsSuccessful == false) { return new UpdateAccountDetailsResponse {IsSuccessful = false, Message = tokenValidateResponse.Message}; }
            if (String.IsNullOrEmpty(tokenValidateResponse.RetrievedEmail)) { return new UpdateAccountDetailsResponse {IsSuccessful = false, Message = AppMessages.NullParameter}; }
            if (String.IsNullOrEmpty(tokenValidateResponse.RetrievedUsername)) { return new UpdateAccountDetailsResponse {IsSuccessful = false, Message = AppMessages.NullParameter}; }

            
            // FIND USER FROM DB
            User? user = await _userManager.FindByNameAsync(tokenValidateResponse.RetrievedEmail);
            
            if (user == null) { user = await _userManager.FindByNameAsync(tokenValidateResponse.RetrievedUsername); }
            
            if (user == null) 
                return new UpdateAccountDetailsResponse
                {
                    Username = null,
                    Email = null,
                    IsSuccessful = false,
                    Message = AppMessages.CannotFindUserToUpdatePasswordFailed
                    
                };
            
            // CHANGE USER'S PASSWORD
            IdentityResult? changedUserResult = await _userManager.ChangePasswordAsync(
                user: user,
                currentPassword: changePasswordRequest.OldPassword,
                newPassword: changePasswordRequest.NewPassword
            );
            
            if (changedUserResult.Succeeded == false)
            {
                return new UpdateAccountDetailsResponse
                {
                    Username = tokenValidateResponse.RetrievedUsername,
                    Email = tokenValidateResponse.RetrievedEmail,
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
                Username = null,
                Email = null,
                IsSuccessful = false,
                Message = ex.Message
                
            };
        }
    }
    
    
    /// <summary>
    /// RESETS THE PASSWORD WHEN ALREADY LOGGED IN
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<InitiatePasswordResetResponse> RequestPasswordResetAsync(InitiatePasswordResetRequest model)
    {
        try
        {
        
            if (string.IsNullOrEmpty(model.Email) == true && string.IsNullOrEmpty(model.Username) == true) 
            { return new InitiatePasswordResetResponse { IsSuccessful = false, Message = AppMessages.NullParameter }; }

            User? user = null;
            
            // FIND USER BY EMAIL
            if (string.IsNullOrEmpty(model.Email) == false) 
            { user = await _userManager.FindByEmailAsync(model.Email); }

            // FIND USER BY USERNAME
            if (string.IsNullOrEmpty(model.Username) == false)
            {
                if (user == null) { user = await _userManager.FindByNameAsync(model.Username); }
            }
            
            // UNABLE TO FIND USER RETURN ERROR MESSAGE
            if (user == null) { return new InitiatePasswordResetResponse { IsSuccessful = false, Message = AppMessages.CannotFindUserToResetPasswordFailed }; }

            // GENERATES TOKEN TO RESET PASSWORD
            // string? passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user: user);
        
            // if (string.IsNullOrEmpty(passwordResetToken) ==  true) { return new InitiatePasswordResetResponse { IsSuccessful = false, Message = AppMessages.GeneratePasswordResetTokenFailed }; }

            // GENERATES ACCESS CODE
            
            // POPULATE SQL TABLE WITH ACCESS CODE
            
            // RETURN ACCESS CODE
            
            return new InitiatePasswordResetResponse { IsSuccessful = true, Message = AppMessages.GeneratePasswordResetTokenSuccess, AccessCode = model};
        }
        catch (Exception e)
        {
            return new()
            {
                IsSuccessful = false,
                Message = e.Message
            };
        }
        
    }

    /// <summary>
    /// RESETS PASSWORD WHEN USER FORGETS IT
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<InitiatePasswordResetResponse> ResetPasswordAsync(RecoverPasswordRequest model)
    {
        try
        {
            // NULL CHECK
            if (string.IsNullOrEmpty(model.Email) == true && 
                string.IsNullOrEmpty(model.Username) == true) 
            { 
                return new() { IsSuccessful = false,
                    Message = AppMessages.NullParameter }; 
            }
            
#pragma warning disable CS8604 // Disable null reference warnings
            // LOOKS ON DATABASE FOR USER USING EMAIL
            User? user = await _userManager.FindByNameAsync(model.Username);
            // LOOKS ON DATABASE FOR USER USING USERNAME
            if (user == null) { user = await _userManager.FindByEmailAsync(model.Email); }
#pragma warning restore CS8604 // Re-enable null reference warnings

            // CHECKS IF USER IS FOUND IN THE DB
            if (user == null) { return new() { IsSuccessful = false, Message = AppMessages.CannotFindUserToResetPasswordFailed }; }

            // LENGTH OF HOW LONG THE ACCESS CODE SHOULD BE
            const int accessCodeLength = 7;
            
            string accessCode = AccessCodeGenerator.GetRandomAccssCode(length: accessCodeLength);
            
            // GENERATE PASSWORD RESET TOKEN
            //string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            TokenResponse? userToken = _tokenProvider.Create(user);
            
            if (userToken == null) { return new InitiatePasswordResetResponse()  { IsSuccessful = false, Message = AppMessages.GeneratePasswordResetTokenFailed }; }
            
            ResetPasswordTokenResponse resetPasswordTokenResponse = new ResetPasswordTokenResponse
            {
                UserToken = userToken.ToString(),
                PassCode = accessCode,
                IsSuccessful = true,
            };

            
            // string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            string body = $"Access Code: {accessCode} JWT Token: {userToken.Token}";
            const string subject = "Reset Password";
            
            // SET EMAIL REQUEST PROPERTIES
            SendEmailRequest sendEmailRequest = new()
            {
                Body = body,
                Subject = subject,
                FromEmail = _emailSettings.HostEmail ??
                            throw new ArgumentNullException(nameof(_emailSettings.HostEmail)),
                Password = _emailSettings.HostEmailPassword ??
                           throw new ArgumentNullException(nameof(_emailSettings.HostEmailPassword)),
                ToEmail = _emailSettings.TestToEmail ??
                          throw new ArgumentNullException(nameof(_emailSettings.TestToEmail)),
                SmtpServer = _emailSettings.HostSmtpServer ??
                             throw new ArgumentNullException(nameof(_emailSettings.HostSmtpServer)),
                SmtpPort = _emailSettings.HostSmtpServerPort
            };
                
            // SENDS AN EMAIL SMTP
            bool emailSentSuccessfully = await SendEmailHelper.SendEmailAsync(sendEmailRequest);
            
            return new InitiatePasswordResetResponse { IsSuccessful = emailSentSuccessfully };
        }
        catch (Exception ex)
        {
            return new InitiatePasswordResetResponse { IsSuccessful = false, Message = ex.Message };
        }
    }

    /// <summary>
    /// ACCEPTS A USERNAME OR EMAIL AS A STRING. GENERATES AN ACCESS CODE FOR RESETTING PASSWORD. WRITE THE ACCESS CODE
    /// TO DATABASE ALONG WITH JWT, EMAIL, USERNAME, EXPIRATION DATETIME.
    /// </summary>
    /// <param name="model">InitiatePasswordResetRequest</param>
    /// <returns>ACCESS CODE, JWT, MESSAGE, SUCCESS BOOLEAN</returns>
    public async Task<InitiatePasswordResetResponse> GetAccessCodeForPasswordChangePassword(InitiatePasswordResetRequest model)
    {
        try
        {
            // NULL CHECKS
            if  (string.IsNullOrEmpty(model.Email) == true && string.IsNullOrEmpty(model.Username) == true)
            { return new() { IsSuccessful = false, Message = AppMessages.NullParameter }; }
            
            User? user = null;
            
            if (string.IsNullOrEmpty(model.Email) == false) 
                { user = await _userManager.FindByEmailAsync(model.Email); }
            
            if (user == null && string.IsNullOrEmpty(model.Username) == false) 
                { user = await _userManager.FindByNameAsync(model.Username); }
            
            if (user == null) { return  new() 
                { IsSuccessful = false, Message = AppMessages.CannotFindUserToResetPasswordFailed }; }
            
            // GENERATE ACCESS CODE
            const int accessCodeLength = 7;
            string accessCode = AccessCodeGenerator.GetRandomAccssCode(length: accessCodeLength);
            
            // WRITE ACCESS CODE TO DATABASE WITH USER ID, ACCESS CODE, EXPIRATION DATE
            user.AccessCode = accessCode;
            user.AccessCodeCreatedAt = DateTime.Now;
            
            // WRITE THE ACCESS CODE TO THE DATABASE
#pragma warning disable CS8602
            await _dbContext.UserTokens.AddAsync(user);
#pragma warning restore CS8602
            await _dbContext.SaveChangesAsync();
            
            return new()
            {
                IsSuccessful = true, 
                Message = AppMessages.GeneratePasswordResetTokenSuccess, 
                AccessCode = accessCode,
                JWTToken = model.JWTToken
            };
            
        }
        catch (Exception e)
        {
            return new()
            {
                IsSuccessful = false,
                Message = e.Message
            };
        }
    }

    /// <summary>
    /// VALIDATES PROVIDED ACCESS CODE. THEN, PROVIDED THEN TAKES THE NEW PASSWORD TO CHANGE THE PASSWORD ON THE DATABASE.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> ValidateAccessCodeAndChangePassword(ResetPasswordRequest model)
    {
        try
        {
            // NULL CHECKS
            if (model == null) { return false; }
            if (string.IsNullOrEmpty(model.AccessCode) ==  true) { return false; }
            if (string.IsNullOrEmpty(model.NewPassword) ==  true) { return false; }
            if (string.IsNullOrEmpty(model.JWTToken) ==  true) { return false; }
            
            // DECODES JWTToken TO FIND USER
            // VALIDATES IF GIVEN JWT IS ACTIVE
            TokenValidateResponse tokenValidateResponse = _tokenProvider.ValidateToken(new UserAuthenticationRequest()
            { JWTToken = model.JWTToken });
            
            // CHECKS IF USER IS FOUND
            if (tokenValidateResponse.IsSuccessful == false) { return false; }
            
            // NULL CHECKS
            if (string.IsNullOrEmpty(tokenValidateResponse.RetrievedEmail) == true) { return false; }
            if (string.IsNullOrEmpty(tokenValidateResponse.RetrievedUsername) == true) { return false; }
            // GET USER ID FROM EMAIL OR USERNAME
            User? foundUser = await _userManager.FindByEmailAsync(tokenValidateResponse.RetrievedEmail) ??
                              await _userManager.FindByNameAsync(tokenValidateResponse.RetrievedUsername);

            // CHECKS IF USER IS NOT FOUND
            if (foundUser == null) { return false; }

            User? foundAccessCodeInstance = null;
            // FIND USER ON USER TOKEN TABLE FROM DATABASE
#pragma warning disable CS8602
            if (_dbContext.UserTokens != null)
            {
                foundAccessCodeInstance = await _dbContext.UserTokens
                    .Where(ut => ut.Id == foundUser.Id)
                    .Where(ut => ut.AccessCode == model.AccessCode)
                    .FirstOrDefaultAsync();
            }
#pragma warning restore CS8602
            
            if (foundAccessCodeInstance == null) { return false; }

            // LOGIN
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// GIVEN AN EMAIL OR USERNAME AND NEW PASSWORD, THIS WILL UPDATE THE DATABASE PASSWORD.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> ChangeUserPassword(ChangePasswordRequest model)
    {
        
    }
}