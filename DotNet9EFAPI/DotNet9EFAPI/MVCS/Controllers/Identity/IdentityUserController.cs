using System.Runtime.CompilerServices;
using DotNet9EFAPI.MVCS.Models._DB.Identity;
using DotNet9EFAPI.MVCS.Models.CRUD.Identity;
using DotNet9EFAPI.MVCS.Services._DB.Identity;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace DotNet9EFAPI.MVCS.Controllers;

[ApiController]
[Route("api/v1/user")]
[AllowAnonymous]
public class IdentityUserAuthentication : ControllerBase
{
    private readonly IIdentityUserService _identityUserSerivce;
    

    public IdentityUserAuthentication(IIdentityUserService identityUserService)
    {
        _identityUserSerivce = identityUserService ?? throw new ArgumentNullException(nameof(identityUserService));
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest loginUserRequest)
    {
        if (loginUserRequest == null) { return BadRequest(AppMessages.NullParameter + nameof(loginUserRequest)); }
        if (loginUserRequest.Username == null) { return BadRequest(AppMessages.NullParameter + nameof(loginUserRequest.Username)); }
        if (loginUserRequest.Password == null) { return BadRequest(AppMessages.NullParameter + nameof(loginUserRequest.Password)); }


        bool isLoggedIn = await _identityUserSerivce.LogInUserAsync(loginUserRequest.Username, loginUserRequest.Password);
        return Ok(isLoggedIn);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> SignUpUser([FromBody] CreateUserRequest createUserRequest)
    {
        // CHECKS FOR NULLS
        if (createUserRequest == null) { return BadRequest(AppMessages.NullParameter + nameof(createUserRequest)); }
        if (createUserRequest.Username == null) { return BadRequest(AppMessages.NullParameter + nameof(createUserRequest.Username)); }
        if (createUserRequest.Email == null) { return BadRequest(AppMessages.NullParameter + nameof(createUserRequest.Email)); }
        if (createUserRequest.Password == null) { return BadRequest(AppMessages.NullParameter + nameof(createUserRequest.Password)); }
        if (createUserRequest.FirstName == null) { return BadRequest(AppMessages.NullParameter + nameof(createUserRequest.FirstName)); }
        if (createUserRequest.LastName == null) { return BadRequest(AppMessages.NullParameter + nameof(createUserRequest.LastName)); }

        // CREATE NEW USER OBJECT TO FOR CREATING USER
        User user = new User()
        {
            UserName = createUserRequest.Username,
            Email = createUserRequest.Email,
            FirstName = createUserRequest.FirstName,
            LastName = createUserRequest.LastName,
            PasswordHash = createUserRequest.Password,
            PhoneNumber = createUserRequest.PhoneNumber
        };

        // CREATES USER ON DATABASE
        bool createUserResult = await _identityUserSerivce.CreateUserAsync(user);

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
    

