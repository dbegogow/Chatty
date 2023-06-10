using FluentValidation;

namespace Chatty.Server.Models.Request.Validators;

public class LoginRequestModelValidator
    : AbstractValidator<LoginRequestModel>
{
    public LoginRequestModelValidator()
    {
        RuleFor(m => m.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(m => m.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}
