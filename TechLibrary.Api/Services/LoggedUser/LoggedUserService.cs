using System.IdentityModel.Tokens.Jwt;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure.DataAccess;

namespace TechLibrary.Api.Services.LoggedUser;

public class LoggedUserService(HttpContext httpContext)
{
    private readonly HttpContext _httpContext = httpContext;

    public User GetUser()
    {
        string authentication = _httpContext.Request.Headers.Authorization.ToString();

        string token = authentication["Bearer ".Length..].Trim();

        JwtSecurityToken jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

        string identifier = jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

        Guid userId = Guid.Parse(identifier);

        TechLibraryDbContext dbContext = new();

        return dbContext.Users.FirstOrDefault(user => user.Id == userId);
    }
}
