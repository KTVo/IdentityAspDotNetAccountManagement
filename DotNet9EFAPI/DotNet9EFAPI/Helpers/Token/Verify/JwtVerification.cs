using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNet9EFAPI.MVCS.Models.CRUD.Identity;
using DotNet9EFAPI.MVCS.Models.JWT;
using Microsoft.IdentityModel.Tokens;

namespace DotNet9EFAPI.Helpers.Token.Verify;

public static class JwtVerification
{
    public static TokenValidateResponse VerifyToken(IConfiguration configuration, UserAuthenticationRequest userAuthenticationRequest)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        TokenValidationParameters validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            ValidIssuer = configuration["Jwt:Issuer"],
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
#pragma warning disable CS8604 // Dereference of a possibly null reference.
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Secret"])),
#pragma warning restore CS8604 // Dereference of a possibly null reference.
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        SecurityToken validatedToken;

        try
        {
            ClaimsPrincipal principal = tokenHandler.ValidateToken(
                userAuthenticationRequest.JWTToken,
                validationParameters,
                out validatedToken
            );
            
            string? userEmail = principal.FindFirst("UserEmail")?.Value;
            string? userName = principal.FindFirst("UserName")?.Value;
            
            if (userEmail == null) { return new TokenValidateResponse { IsSuccessful = false }; }
            
            return new TokenValidateResponse
            {
                IsSuccessful = true,
                ValidatedToken = validatedToken,
                ClaimsPrincipal = principal,
                RetrievedEmail = userEmail,
                RetrievedUsername = userName,
                Message = "TOKEN VALIDATED SUCCESSFULLY."
             };
        }
        catch (SecurityTokenExpiredException ex)
        {
            // HANDLE EXPIRED TOKENS
            return new TokenValidateResponse
            {
                IsSuccessful = false,
                ValidatedToken = null,
                ClaimsPrincipal = null,
                Message = ex.Message
             };
        }
        catch (Exception ex)
        {
            // HANDLE OTHER VALIDATION ERRORS (E.G. BAD SIGNATURE, WRONG AUDIENCE, ETC)
            return new TokenValidateResponse
            {
                IsSuccessful = false,
                ValidatedToken = null,
                ClaimsPrincipal = null,
                Message = ex.Message
             };
        }
    }
}
