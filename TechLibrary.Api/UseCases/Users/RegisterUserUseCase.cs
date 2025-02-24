using FluentValidation.Results;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure.DataAccess;
using TechLibrary.Api.Infraestructure.Security.Criptography;
using TechLibrary.Api.Infraestructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Users;

public class RegisterUserUseCase
{
    public ResponseUserJson Execute(RequestUserJson request)
    {
        TechLibraryDbContext dbContext = new();

        Validate(request, dbContext);

        User entity = new()
        {
            Name = request.Name,
            Email = request.Email,
            Password = BCryptAlgorithm.HashPassword(request.Password),
        };
        
        dbContext.Users.Add(entity);
        dbContext.SaveChanges();

        return new ResponseUserJson()
        {
            Name = entity.Name,
            AccessToken = AccessTokenGenerator.Generate(entity)
        };
    }

    private static void Validate(RequestUserJson request, TechLibraryDbContext dbContext)
    {
        RegisterUserValidator validator = new();

        ValidationResult result = validator.Validate(request);

        if (dbContext.Users.Any(user => user.Email.Equals(request.Email))) result.Errors.Add(new ValidationFailure("Email", "E-mail já cadastrado no sistema"));

        if (!result.IsValid)
        {
            List<string> errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
