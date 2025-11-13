using DotNet9EFAPI.MVCS.Models.CRUD.AccountManagement;
using DotNet9EFAPI.MVCS.Models.DummyData;
using DotNet9EFAPI.MVCS.Models.Email;
using DotNet9EFAPI.MVCS.Models.JWT;
using DotNet9EFAPI.MVCS.Services._DB.AccountManagement;
using DotNet9EFAPI.MVCS.Services._DB.JWT;
using DotNet9EFAPI.MVCS.Services.Email;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace DotNet9EFAPI.MVCS.Controllers;

[AllowAnonymous]
[Route("api/v1/account/update")]
[ApiController]
public class RegisterAccountService : ControllerBase
{
    private readonly IUpdateAccountDetailsService _updateAccountDetailsService;
    
    private readonly ITokenProvider _tokenProvider;
    private readonly ISmtpEmailService _emailSender;


    public RegisterAccountService(
        ISmtpEmailService emailSender,
        IUpdateAccountDetailsService updateAccountDetailsService, 
        ITokenProvider tokenProvider
        )
    {
        _updateAccountDetailsService = updateAccountDetailsService ?? throw new ArgumentNullException(nameof(updateAccountDetailsService));
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        _emailSender = emailSender  ?? throw new ArgumentNullException(nameof(emailSender));
    }
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userAuthenticationRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("test/token")]
    public IActionResult TestToken([FromBody] UserAuthenticationRequest? userAuthenticationRequest)
    {
        if (userAuthenticationRequest == null) { return BadRequest(AppMessages.NullParameter + nameof(userAuthenticationRequest)); }
        if (userAuthenticationRequest.JWTToken == null) { return BadRequest(AppMessages.NullParameter + nameof(userAuthenticationRequest.JWTToken)); }

        TokenValidateResponse response = _tokenProvider.ValidateToken(userAuthenticationRequest);
        
        return Ok(response);
    }

    [HttpPost]
    [Route("text/todo")]
    public async Task<IActionResult> ToDo([FromBody] UserAuthenticationRequest? userAuthenticationRequest)
    {
        if (userAuthenticationRequest == null) { return BadRequest(AppMessages.NullParameter + nameof(userAuthenticationRequest)); }
        if (userAuthenticationRequest.USToken == null) { return BadRequest(AppMessages.NullParameter + nameof(userAuthenticationRequest.USToken)); }

        ToDoResponse? response = await _tokenProvider.AuthorizationTestAsync(userAuthenticationRequest);

        if (response == null) { return BadRequest(false); }
        return Ok(response);
    }
    
    [HttpPost]
    [Route("update/email")]
    public async Task<IActionResult> UpdateUserEmail([FromBody] ChangeEmailRequest? changeEmailRequest)
    {
        if (changeEmailRequest == null) { return BadRequest(AppMessages.NullParameter + nameof(changeEmailRequest)); }
        if (changeEmailRequest.JWTToken == null) { return BadRequest(AppMessages.NullParameter + nameof(changeEmailRequest.JWTToken)); }

        UpdateAccountDetailsResponse updateAccountDetailsResponse = await _updateAccountDetailsService.UpdateUserEmailAsync(changeEmailRequest);
        
        if (updateAccountDetailsResponse.IsSuccessful == false) { return BadRequest(updateAccountDetailsResponse); }
        
        return Ok(updateAccountDetailsResponse);
    }
    
    [HttpPost]
    [Route("update/phone/number")]
    public async Task<IActionResult> UpdateUserPhoneNumber([FromBody] ChangePhoneNumberRequest? changePhoneNumberRequest)
    {
        if (changePhoneNumberRequest == null) { return BadRequest(AppMessages.NullParameter + nameof(changePhoneNumberRequest)); }
        if (changePhoneNumberRequest.JWTToken == null) { return BadRequest(AppMessages.NullParameter + nameof(changePhoneNumberRequest.JWTToken)); }

        UpdateAccountDetailsResponse updateAccountDetailsResponse = await _updateAccountDetailsService.UpdatePhoneNumberAsync(changePhoneNumberRequest);
        
        if (updateAccountDetailsResponse.IsSuccessful == false) { return BadRequest(updateAccountDetailsResponse); }
        
        return Ok(updateAccountDetailsResponse);
    }
    
    [HttpPost]
    [Route("update/details")]
    public async Task<IActionResult> UpdateUserDetail([FromBody] ChangeUserRequest? changeUserRequest)
    {
        // NULL CHECKS - WILL CHECK FOR OTHER FIELDS IN THE SERVICE
        if (changeUserRequest == null) { return BadRequest(AppMessages.NullParameter + nameof(changeUserRequest)); }
        if (changeUserRequest.JWTToken == null) { return BadRequest(AppMessages.NullParameter + nameof(changeUserRequest.JWTToken)); }

        UpdateAccountDetailsResponse updateAccountDetailsResponse = await _updateAccountDetailsService.UpdateUserAsync(changeUserRequest);
        
        if (updateAccountDetailsResponse.IsSuccessful == false) { return BadRequest(updateAccountDetailsResponse); }
        
        return Ok(updateAccountDetailsResponse);
    }
    
    [HttpPost]
    [Route("test/send/email")]
    public async Task<IActionResult> TestSendEmail()
    {
        SendEmailResponse initialResetPasswordResponse = await _emailSender.SendEmailAsync();
        
        if (initialResetPasswordResponse.IsSuccessful == false) { return BadRequest(initialResetPasswordResponse); }
        
        return Ok(initialResetPasswordResponse);
    }
}
    

