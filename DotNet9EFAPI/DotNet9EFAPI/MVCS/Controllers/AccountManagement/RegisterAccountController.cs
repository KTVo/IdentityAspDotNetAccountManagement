using DotNet9EFAPI.MVCS.Models._DB.AccountManagement;
using DotNet9EFAPI.MVCS.Models.CRUD.AccountManagement;
using DotNet9EFAPI.MVCS.Services._DB.AccountManagement;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet9EFAPI.MVCS.Controllers;

[AllowAnonymous]
[Route("api/v1/account/register")]
[ApiController]
public class RegisterAccountController : ControllerBase
{
    private readonly IRegistrationAccountService _registrationAccountSerivce;


    public RegisterAccountController(IRegistrationAccountService registrationAccountSerivce)
    {
        _registrationAccountSerivce =  registrationAccountSerivce ?? throw new ArgumentNullException(nameof(registrationAccountSerivce));
    }
    

    /// <summary>
    /// REGISTER USER USING IDENTITY AND DATABASE
    /// </summary>
    /// <param name="createUserRequest"></param>
    /// <returns></returns>
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
        bool createUserResult = await _registrationAccountSerivce.CreateUserAsync(user);

        // INSTANTIATE RESPONSE CLASS
        CreateUserResponse createUserResponse = new CreateUserResponse();

        // IF IDENTITY FAILS TO CREATE A USER ON DATABASE
        if (createUserResult == false)
        {
            createUserResponse.IsSuccessful = false;
            createUserResponse.Message = AppMessages.CreatingUserFailed;

            return BadRequest(createUserResponse);
        }

        // RETURNS CREATED USERS SUCCESS OBJECT
        createUserResponse.IsSuccessful = true;
        createUserResponse.Message = AppMessages.CreatedUserSuccess;

        return Ok(createUserResponse);
    }
}