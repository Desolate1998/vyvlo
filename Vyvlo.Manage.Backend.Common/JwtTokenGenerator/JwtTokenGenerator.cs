using Common.DateTimeProvider;
using Common.JwtTokenGenerator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace Vyvlo.Manage.Backend.Common.JwtTokenGenerator;

public class JwtTokenGenerator(IOptions<JwtSettings> jwtSettings) : IJwtTokenGenerator
{
    string IJwtTokenGenerator.GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    string IJwtTokenGenerator.GenerateToken(string email, string firstName, string lastName, Guid userId)
    {
        SigningCredentials signingCredentials = new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Secret)),
            SecurityAlgorithms.HmacSha256
        );

        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.GivenName, firstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, lastName),
            new Claim(JwtRegisteredClaimNames.Sid, userId.ToString()),

        ];

        JwtSecurityToken securityToken = new(
            issuer: jwtSettings.Value.Issuer,
            audience: jwtSettings.Value.Audience,
            expires: DateTimeProvider.ApplicationDate.AddMinutes(jwtSettings.Value.ExpiryMinutes),
            claims: claims, signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
