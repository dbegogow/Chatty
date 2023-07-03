using FluentValidation;

namespace Chatty.Server.Models.Request.Validators;

public class MessageRequestModelValidator
    : AbstractValidator<MessageRequestModel>
{
    public MessageRequestModelValidator()
    {
        RuleFor(m => m.Text)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(m => m.ReceiverUsername)
            .NotEmpty()
            .Length(3, 20);
    }
}
