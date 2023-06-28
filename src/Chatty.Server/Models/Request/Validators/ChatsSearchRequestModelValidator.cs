using FluentValidation;

namespace Chatty.Server.Models.Request.Validators;

public class ChatsSearchRequestModelValidator
    : AbstractValidator<ChatsSearchRequestModel>
{
    public ChatsSearchRequestModelValidator()
    {
        RuleFor(m => m.Skip)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(1000);

        RuleFor(m => m.Take)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(1000);
    }
}
