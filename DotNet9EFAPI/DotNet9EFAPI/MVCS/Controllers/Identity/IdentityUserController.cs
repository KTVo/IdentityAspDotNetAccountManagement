using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNet9EFAPI.MVCS.Models._DB.Identity;
using DotNet9EFAPI.MVCS.Models.CRUD.Identity;
using DotNet9EFAPI.MVCS.Services._DB.Identity;
using DotNet9EFAPI.MVCS.Services._DB.JWT;
using DotNet9EFAPI.Statics.Messages.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        string? userToken = await _identityUserSerivce.LogInUserAsync(loginUserRequest.Username, loginUserRequest.Password);
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

    [HttpPost]
    [Route("test/token")]
    public IActionResult TestToken([FromBody] UserAuthenticationRequest userAuthenticationRequest)
    {
        if (userAuthenticationRequest == null) { return BadRequest(AppMessages.NullParameter + nameof(userAuthenticationRequest)); }
        if (userAuthenticationRequest.USToken == null) { return BadRequest(AppMessages.NullParameter + nameof(userAuthenticationRequest.USToken)); }

        string? response = _tokenProvider.TestToken(userAuthenticationRequest);

        if (response == null) { return BadRequest(false); }
        return Ok(response);
    }
}
    

