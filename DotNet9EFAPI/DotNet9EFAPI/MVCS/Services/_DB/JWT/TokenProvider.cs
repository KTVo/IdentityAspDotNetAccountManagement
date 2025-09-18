using System.Security.Claims;
using System.Text;
using DotNet9EFAPI.MVCS.Models._DB.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Diagnostics;
using DotNet9EFAPI.MVCS.Models.CRUD.Identity;

namespace DotNet9EFAPI.MVCS.Services._DB.JWT;

// TODO: LOOK UP WHAT DOES "internal sealed class" DO
internal sealed class TokenProvider : ITokenProvider
{
    private readonly IConfiguration? _configuration;

    public TokenProvider(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new Exception(nameof(configuration));
    }

    public string? Create(User user)
    {
        if (user.Email == null) { return null; }

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

        return token;
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
}
