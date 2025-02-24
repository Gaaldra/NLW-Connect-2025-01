using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechLibrary.Api.Domain.Entities;

namespace TechLibrary.Api.Infraestructure.Security.Tokens.Access;

public class AccessTokenGenerator
{
    public static string Generate(User user)
    {
        List<Claim> claims = new()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        };

        SecurityTokenDescriptor descriptor = new()
        {
            Expires = DateTime.UtcNow.AddHours(1),
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature )
        };

        JwtSecurityTokenHandler handler = new();

        SecurityToken securityToken = handler.CreateToken(descriptor);

        return handler.WriteToken(securityToken);
    }

    private static SymmetricSecurityKey SecurityKey()
    {
        string signingKey = "xTlBerQ0Bc88bcbrSxuqvPTAX5aBsMx3";

        byte[] symmetricKey = Encoding.UTF8.GetBytes(signingKey);

        return new SymmetricSecurityKey(symmetricKey);
    }
}
