using DotNet9EFAPI.MVCS.Models.CRUD.Identity;
using DotNet9EFAPI.MVCS.Services._DB.Identity;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet9EFAPI.MVCS.Controllers;

[ApiController]
[Route("api/v1/test/user")]
[AllowAnonymous]
public class IdentityUserAuthentication : ControllerBase
{
    private readonly IIdentityUserService _identityUserSerivce;
    

    public IdentityUserAuthentication(IIdentityUserService identityUserService)
    {
        _identityUserSerivce = identityUserService ?? throw new ArgumentNullException(nameof(identityUserService));
    }

    [HttpPost]
    [Route("register/user")]
    public async Task<IActionResult> SignUpUser([FromBody] CreateUserRequest createUserRequest)
    {
        // CHECKS FOR NULLS
        if (createUserRequest.User == null) { return BadRequest(AppMessages.NullParameter); }
        if (createUserRequest.User.PasswordHash == null) { return BadRequest(AppMessages.NullPasswordParameter); }

        // CREATES USER ON DATABASE
        bool createUserResult = await _identityUserSerivce.CreateUserAsync(createUserRequest.User);

        // INSTANTIATE RESPONSE CLASS
        CreateUserResponse createUserResponse = new CreateUserResponse();

        // IF IDENTITY FAILS TO CREATE A USER ON DATABASE
        if (createUserResult == false)
        {
            createUserResponse.IsSuccessful = false;
            createUserResponse.StatusCode = 500;
            createUserResponse.Message = AppMessages.CreatingUserFailed;

            return BadRequest(createUserResponse);
        }

        // RETURNS CREATED USERS SUCCESS OBJECT
        createUserResponse.IsSuccessful = true;
        createUserResponse.StatusCode = 200;
        createUserResponse.Message = AppMessages.CreatedUserSuccess;
        
        return Ok(createUserResponse);
    }
}
    

