using DotNet9EFAPI.Helpers.Email;
using DotNet9EFAPI.MVCS.Models._DB.AccountManagement;
using DotNet9EFAPI.MVCS.Models.CRUD.AccountManagement;
using DotNet9EFAPI.MVCS.Models.Email;
using DotNet9EFAPI.MVCS.Models.JWT;
using DotNet9EFAPI.MVCS.Services._DB.JWT;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DotNet9EFAPI.MVCS.Services._DB.AccountManagement;

public class PasswordResetService : IPasswordResetService
{
    // IDENTITY THAT OBJECT FOR MANAGING USER
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly EmailSettings _emailSettings;

    // CONSTRUCTOR
    public PasswordResetService(
        UserManager<User> userManager, 
        ITokenProvider tokenProvider,
        IOptions<EmailSettings> emailSettings)
    {
        // ASSIGNS DEPENDENCY INJECTED IDENTITY INSTANCES TO CLASS VARIABLE
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        _emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings));
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
        if (model.JWTToken == null) { return new InitiatePasswordResetResponse { IsSuccessful = false, Message = AppMessages.NullParameter }; }
        
        // VALIDATES IF GIVEN JWT IS ACTIVE
        TokenValidateResponse tokenValidateResponse = _tokenProvider.ValidateToken(new UserAuthenticationRequest()
        {
            JWTToken = model.JWTToken
        });
        
        if (string.IsNullOrEmpty(tokenValidateResponse.RetrievedEmail) == true) { return new InitiatePasswordResetResponse { IsSuccessful = false, Message = AppMessages.NullParameter }; }
        User? user = await _userManager.FindByEmailAsync(tokenValidateResponse.RetrievedEmail);
        
        if (user == null) { return new InitiatePasswordResetResponse { IsSuccessful = false, Message = AppMessages.CannotFindUserToResetPasswordFailed }; }

        // GENERATES TOKEN TO RESET PASSWORD
        string? passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user: user);
        
        if (string.IsNullOrEmpty(passwordResetToken) ==  true) { return new InitiatePasswordResetResponse { IsSuccessful = false, Message = AppMessages.GeneratePasswordResetTokenFailed }; }

        return new InitiatePasswordResetResponse { IsSuccessful = true, Message = AppMessages.GeneratePasswordResetTokenSuccess, PasswordResetToken = passwordResetToken };
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
            // VALIDATES IF GIVEN JWT IS ACTIVE
            TokenValidateResponse tokenValidateResponse = _tokenProvider.ValidateToken(new UserAuthenticationRequest()
            {
                JWTToken = model.JWTToken
            });
            
            if (string.IsNullOrEmpty(tokenValidateResponse.RetrievedEmail) == true && string.IsNullOrEmpty(tokenValidateResponse.RetrievedUsername) == true) { return new() { IsSuccessful = false,
                Message = AppMessages.NullParameter }; }
            
#pragma warning disable CS8604 // Disable null reference warnings
            // LOOKS ON DATABASE FOR USER USING EMAIL
            User? user = await _userManager.FindByNameAsync(tokenValidateResponse.RetrievedUsername);
            // LOOKS ON DATABASE FOR USER USING USERNAME
            if (user == null) { user = await _userManager.FindByEmailAsync(tokenValidateResponse.RetrievedEmail); }
#pragma warning restore CS8604 // Re-enable null reference warnings

            // CHECKS IF USER IS FOUND IN THE DB
            if (user == null) { return new() { IsSuccessful = false, Message = AppMessages.CannotFindUserToResetPasswordFailed }; }
            
            // GENERATE PASSWORD RESET TOKEN
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            TokenResponse? userToken = _tokenProvider.Create(user);
            
            if (userToken == null) { return new InitiatePasswordResetResponse()  { IsSuccessful = false, Message = AppMessages.GeneratePasswordResetTokenFailed }; }
            
            ResetPasswordTokenResponse resetPasswordTokenResponse = new ResetPasswordTokenResponse
            {
                UserToken = userToken.ToString(),
                PassCode = token,
                IsSuccessful = true,
            };

            
            // string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            string body = $"email: {tokenValidateResponse.RetrievedEmail} jwtToken: {userToken.Token}";
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
}