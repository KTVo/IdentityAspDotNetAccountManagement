using System.Security.Claims;
using System.Text;
using DotNet9EFAPI.MVCS.Models._DB.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Diagnostics;
using DotNet9EFAPI.MVCS.Models.CRUD.Identity;
using DotNet9EFAPI.MVCS.Models.JWT;
using DotNet9EFAPI.Statics.Messages.App;
using DotNet9EFAPI.MVCS.Models.DummyData;
using DotNet9EFAPI.MVCS.Services.REST;
using System.Threading.Tasks;

namespace DotNet9EFAPI.MVCS.Services._DB.JWT;

// TODO: LOOK UP WHAT DOES "internal sealed class" DO
internal sealed class TokenProvider : ITokenProvider
{
    private readonly IConfiguration? _configuration;
    private readonly IRestService? _restService;


    public TokenProvider(IConfiguration configuration, IRestService restService)
    {
        _configuration = configuration ?? throw new Exception(nameof(configuration));
        _restService = restService ?? throw new Exception(nameof(restService));
    }

    public TokenResponse Create(User user)
    {
        try
        {
            if (user == null) { return new TokenResponse { Token = null, IsSuccessful = false, Message = AppMessages.NullModel }; }
            if (user.Email == null) { return new TokenResponse { Token = null, IsSuccessful = false, Message = AppMessages.NullParameter }; }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string secretKey = _configuration["Jwt:Secret"]!;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = new[]
            {
                new Claim("UserEmail", user.Email)
            };

            JwtSecurityToken Sectoken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(_configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
                signingCredentials: credentials);

            string token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return new TokenResponse { Token = token, IsSuccessful = true, Message = AppMessages.CreatedTokenSuccess };
        }
        catch (Exception ex)
        {
            return new TokenResponse { Token = null, IsSuccessful = false, Message = AppMessages.CreatedTokenFailed + " --> " + ex.Message };
        }

    }

    public string? TestToken(UserAuthenticationRequest userAuthenticationRequest)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                ValidIssuer = _configuration["Jwt:Issuer"],
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
#pragma warning disable CS8604 // Dereference of a possibly null reference.
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"])),
#pragma warning restore CS8604 // Dereference of a possibly null reference.
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            SecurityToken validatedToken;
            ClaimsPrincipal principal = tokenHandler.ValidateToken(userAuthenticationRequest.USToken, validationParameters, out validatedToken);

            string? userEmail = principal.FindFirst("UserEmail")?.Value;

            if (userEmail == null) { return null; }

            return userEmail;
        }
        catch (SecurityTokenValidationException ex)
        {
            Debug.WriteLine($"Token validation failed: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }
    
    public async Task<ToDoResponse?> AuthorizationTestAsync(UserAuthenticationRequest userAuthenticationRequest)
    {

        try
        {
#pragma warning disable CS8604 // Possible null reference argument.
            TokenValidateResponse tokenValidateResponse = Helpers.Token.Verify.JwtVerification.VerifyToken(_configuration: _configuration, userAuthenticationRequest);
#pragma warning restore CS8604 // Possible null reference argument.
            if (tokenValidateResponse.IsSuccessful == false) { return new ToDoResponse { IsSuccessful = false, Message = tokenValidateResponse.Message }; }


#pragma warning disable CS8602 // Dereference of a possibly null reference.
            ToDoResponse? restResponse = await _restService.GetDataAsync("https://jsonplaceholder.typicode.com/todos");
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            if (restResponse == null) { return new ToDoResponse { IsSuccessful = false, Message = AppMessages.NullResponse }; }

            return restResponse;
        }
        catch (SecurityTokenValidationException ex)
        {
            return new ToDoResponse { IsSuccessful = false, Message = ex.Message }; 
        }
        catch (Exception ex)
        {
            return new ToDoResponse { IsSuccessful = false, Message = ex.Message }; 
        }
    }
}
