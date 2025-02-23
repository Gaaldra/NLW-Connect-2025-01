using FluentValidation;
using TechLibrary.Communication.Requests;

namespace TechLibrary.Api.UseCases.Users;

public class RegisterUserValidator : AbstractValidator<RequestUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage("Nome do usuário é obrigatório");
        RuleFor(request => request.Email).EmailAddress().WithMessage("Email não é válido");
        RuleFor(request => request.Password).NotEmpty().WithMessage("Senha é obrigatório");
        When(request => string.IsNullOrEmpty(request.Password) == false, () =>
        {
            RuleFor(request => request.Password.Length).GreaterThanOrEqualTo(8).WithMessage("Senha precisa ter o mínimo de 8 caracteres.");
        });
    }
}
