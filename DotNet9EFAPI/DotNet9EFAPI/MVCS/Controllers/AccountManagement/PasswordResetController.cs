using DotNet9EFAPI.MVCS.Models.CRUD.AccountManagement;
using DotNet9EFAPI.MVCS.Services._DB.AccountManagement;
using DotNet9EFAPI.MVCS.Services._DB.JWT;
using DotNet9EFAPI.MVCS.Services.Email;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet9EFAPI.MVCS.AccountManagement.Controllers;

[AllowAnonymous]
[Route("api/v1/account/password/management")]
[ApiController]
public class PasswordResetController : ControllerBase
{
    private readonly IPasswordResetService _passwordResetService;
    
    public PasswordResetController(
        IPasswordResetService passwordResetService
    )
    {
        _passwordResetService =  passwordResetService ?? throw new ArgumentNullException(nameof(passwordResetService));
    }
    
    [HttpPost]
    [Route("update/password")]
    public async Task<IActionResult> UpdateUserPassword([FromBody] ChangePasswordRequest? changePasswordRequest)
    {
        if (changePasswordRequest == null) { return BadRequest(AppMessages.NullParameter + nameof(changePasswordRequest)); }
        if (changePasswordRequest.JWTToken == null) { return BadRequest(AppMessages.NullParameter + nameof(changePasswordRequest.JWTToken)); }

        UpdateAccountDetailsResponse updateAccountDetailsResponse = await _passwordResetService.UpdateUserPasswordAsync(changePasswordRequest);
        
        if (updateAccountDetailsResponse.IsSuccessful == false) { return BadRequest(updateAccountDetailsResponse); }
        
        return Ok(updateAccountDetailsResponse);
    }
    
    [HttpPost]
    [Route("reset/password")]
    public async Task<IActionResult> ResetPassword([FromBody] RecoverPasswordRequest recoverPasswordRequest)
    {
        if (recoverPasswordRequest == null) { return BadRequest(AppMessages.NullParameter + nameof(recoverPasswordRequest)); }
        if (recoverPasswordRequest.JWTToken == null) { return BadRequest(AppMessages.NullParameter + nameof(recoverPasswordRequest.JWTToken)); }

        InitiatePasswordResetResponse initialResetPasswordResponse = await _passwordResetService.ResetPasswordAsync(recoverPasswordRequest);
        
        if (initialResetPasswordResponse.IsSuccessful == false) { return BadRequest(initialResetPasswordResponse); }
        
        return Ok(initialResetPasswordResponse);
    }
    
    [HttpPost]
    [Route("request/reset/password")]
    public async Task<IActionResult> RequestPasswordReset(InitiatePasswordResetRequest model)
    {
        var initialResetPasswordResponse = await _passwordResetService.RequestPasswordResetAsync(model);
        
        if (initialResetPasswordResponse.IsSuccessful == false) { return BadRequest(initialResetPasswordResponse); }
        
        return Ok(initialResetPasswordResponse);
    }
}