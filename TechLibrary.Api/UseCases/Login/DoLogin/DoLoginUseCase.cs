using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure.DataAccess;
using TechLibrary.Api.Infraestructure.Security.Criptography;
using TechLibrary.Api.Infraestructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Login.DoLogin;

public class DoLoginUseCase
{
    public ResponseUserJson Execute(RequestLoginJson request)
    {
        TechLibraryDbContext dbContext = new();

        User entity = dbContext.Users.FirstOrDefault(user => user.Email.Equals(request.Email)) ?? throw new InvalidLoginException();

        if (BCryptAlgorithm.VerifyPassword(request.Password, entity.Password)) throw new InvalidLoginException();

        return new ResponseUserJson
        {
            Name = entity.Name,
            AccessToken = AccessTokenGenerator.Generate(entity)
        };
    }
}
