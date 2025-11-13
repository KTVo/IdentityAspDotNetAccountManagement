using DotNet9EFAPI.MVCS.Models.CRUD.AccountManagement;
using DotNet9EFAPI.MVCS.Models.JWT;
using DotNet9EFAPI.MVCS.Services._DB.AccountManagement;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet9EFAPI.MVCS.Controllers;

[AllowAnonymous]
[Route("api/v1/account/login")]
[ApiController]
public class LoginAccountController : ControllerBase
{

    private readonly ILoginAccountService _loginAccountSerivce;

    public LoginAccountController(ILoginAccountService  loginAccountSerivce)
    {
        _loginAccountSerivce = loginAccountSerivce ?? throw new ArgumentNullException(nameof(loginAccountSerivce));
    }

    /// <summary>
    /// LOGINS IN USER VIA USERNAME AND PASSWORD USING IDENTITY AND DATABASE
    /// </summary>
    /// <param name="loginUserRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest loginUserRequest)
    {
        if (loginUserRequest == null) { return BadRequest(AppMessages.NullParameter + nameof(loginUserRequest)); }
        if (loginUserRequest.Password == null) { return BadRequest(AppMessages.NullParameter + nameof(loginUserRequest.Password)); }

        string chosenUsername = loginUserRequest.Username ?? loginUserRequest.Email ?? string.Empty;
        if (chosenUsername == String.Empty) { return BadRequest(AppMessages.NullParameter + nameof(chosenUsername)); }
        
        TokenResponse? userToken = await _loginAccountSerivce.LogInUserAsync(chosenUsername, loginUserRequest.Password);

        return Ok(userToken);
    }
}