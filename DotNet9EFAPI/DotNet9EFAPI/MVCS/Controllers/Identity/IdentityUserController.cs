using DotNet9EFAPI.MVCS.Models._DB.Identity;
using DotNet9EFAPI.MVCS.Models.CRUD.Identity;
using DotNet9EFAPI.MVCS.Models.DummyData;
using DotNet9EFAPI.MVCS.Models.JWT;
using DotNet9EFAPI.MVCS.Services._DB.Identity;
using DotNet9EFAPI.MVCS.Services._DB.JWT;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.AspNetCore.Mvc;
namespace DotNet9EFAPI.MVCS.Controllers;

[Route("api/v1/user")]
[ApiController]
public class IdentityUserAuthentication : ControllerBase
{
    private readonly IIdentityUserService _identityUserSerivce;
    public readonly ITokenProvider _tokenProvider;


    public IdentityUserAuthentication(IIdentityUserService identityUserService, ITokenProvider tokenProvider, IConfiguration configuration)
    {
        _identityUserSerivce = identityUserService ?? throw new ArgumentNullException(nameof(identityUserService));
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest loginUserRequest)
    {
        if (loginUserRequest == null) { return BadRequest(AppMessages.NullParameter + nameof(loginUserRequest)); }
        if (loginUserRequest.Username == null) { return BadRequest(AppMessages.NullParameter + nameof(loginUserRequest.Username)); }
        if (loginUserRequest.Password == null) { return BadRequest(AppMessages.NullParameter + nameof(loginUserRequest.Password)); }

        TokenResponse? userToken = await _identityUserSerivce.LogInUserAsync(loginUserRequest.Username, loginUserRequest.Password);

        return Ok(userToken);
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
            createUserResponse.Message = AppMessages.CreatingUserFailed;

            return BadRequest(createUserResponse);
        }

        // RETURNS CREATED USERS SUCCESS OBJECT
        createUserResponse.IsSuccessful = true;
        createUserResponse.Message = AppMessages.CreatedUserSuccess;

        return Ok(createUserResponse);
    }

    [HttpPost]
    [Route("test/token")]
    public IActionResult TestToken([FromBody] UserAuthenticationRequest? userAuthenticationRequest)
    {
        if (userAuthenticationRequest == null) { return BadRequest(AppMessages.NullParameter + nameof(userAuthenticationRequest)); }
        if (userAuthenticationRequest.USToken == null) { return BadRequest(AppMessages.NullParameter + nameof(userAuthenticationRequest.USToken)); }

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
    [Route("update/user/password")]
    public async Task<IActionResult> UpdateUserPassword([FromBody] ChangePasswordRequest? changePasswordRequest)
    {
        if (changePasswordRequest == null) { return BadRequest(AppMessages.NullParameter + nameof(changePasswordRequest)); }
        if (changePasswordRequest.JWTToken == null) { return BadRequest(AppMessages.NullParameter + nameof(changePasswordRequest.JWTToken)); }

        UpdateAccountDetailsResponse updateAccountDetailsResponse = await _identityUserSerivce.UpdateUserPasswordAsync(changePasswordRequest);
        
        if (updateAccountDetailsResponse.IsSuccessful == false) { return BadRequest(updateAccountDetailsResponse); }
        
        return Ok(updateAccountDetailsResponse);
    }
    
    [HttpPost]
    [Route("update/user/email")]
    public async Task<IActionResult> UpdateUserEmail([FromBody] ChangeEmailRequest? changeEmailRequest)
    {
        if (changeEmailRequest == null) { return BadRequest(AppMessages.NullParameter + nameof(changeEmailRequest)); }
        if (changeEmailRequest.JWTToken == null) { return BadRequest(AppMessages.NullParameter + nameof(changeEmailRequest.JWTToken)); }

        UpdateAccountDetailsResponse updateAccountDetailsResponse = await _identityUserSerivce.UpdateUserEmailAsync(changeEmailRequest);
        
        if (updateAccountDetailsResponse.IsSuccessful == false) { return BadRequest(updateAccountDetailsResponse); }
        
        return Ok(updateAccountDetailsResponse);
    }
    
    [HttpPost]
    [Route("update/user/phone/number")]
    public async Task<IActionResult> UpdateUserPhoneNumber([FromBody] ChangePhoneNumberRequest? changePhoneNumberRequest)
    {
        if (changePhoneNumberRequest == null) { return BadRequest(AppMessages.NullParameter + nameof(changePhoneNumberRequest)); }
        if (changePhoneNumberRequest.JWTToken == null) { return BadRequest(AppMessages.NullParameter + nameof(changePhoneNumberRequest.JWTToken)); }

        UpdateAccountDetailsResponse updateAccountDetailsResponse = await _identityUserSerivce.UpdatePhoneNumberAsync(changePhoneNumberRequest);
        
        if (updateAccountDetailsResponse.IsSuccessful == false) { return BadRequest(updateAccountDetailsResponse); }
        
        return Ok(updateAccountDetailsResponse);
    }
    
    [HttpPost]
    [Route("update/user/details")]
    public async Task<IActionResult> UpdateUserDetail([FromBody] ChangeUserRequest? changeUserRequest)
    {
        if (changeUserRequest == null) { return BadRequest(AppMessages.NullParameter + nameof(changeUserRequest)); }
        if (changeUserRequest.JWTToken == null) { return BadRequest(AppMessages.NullParameter + nameof(changeUserRequest.JWTToken)); }

        UpdateAccountDetailsResponse updateAccountDetailsResponse = await _identityUserSerivce.UpdateUserAsync(changeUserRequest);
        
        if (updateAccountDetailsResponse.IsSuccessful == false) { return BadRequest(updateAccountDetailsResponse); }
        
        return Ok(updateAccountDetailsResponse);
    }
}
    

