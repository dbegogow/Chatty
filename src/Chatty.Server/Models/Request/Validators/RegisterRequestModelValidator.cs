using FluentValidation;

namespace Chatty.Server.Models.Request.Validators;

public class RegisterRequestModelValidator
    : AbstractValidator<RegisterRequestModel>
{
    public RegisterRequestModelValidator()
    {
        RuleFor(m => m.Username)
            .NotEmpty()
            .Length(3, 20);

        RuleFor(m => m.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(m => m.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}
