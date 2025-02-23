using FluentValidation.Results;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructures;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Users;

public class RegisterUserUseCase
{
    public ResponseUserJson Execute(RequestUserJson request)
    {
        Validate(request);

        User entity = new()
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password,
        };

        TechLibraryDbContext dbContext = new();

        dbContext.Users.Add(entity);
        dbContext.SaveChanges();

        return new ResponseUserJson()
        {
            Name = entity.Name
        };
    }

    private static void Validate(RequestUserJson request)
    {
        RegisterUserValidator validator = new();

        ValidationResult result = validator.Validate(request);

        if (!result.IsValid)
        {
            List<string> errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
