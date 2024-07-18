using FluentValidation;
using ThreadBasket.Application.Features.DmcThread.Models;

namespace ThreadBasket.Application.Features.DmcThread.Validators;

public class CreateDmcThreadValidator : AbstractValidator<CreateDmcThreadRequest>
{
    public CreateDmcThreadValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Floss)
            .NotEmpty()
            .MaximumLength(20);

        When(x => !string.IsNullOrEmpty(x.WebColor), () =>
        {
            RuleFor(x => x.WebColor)
                .MaximumLength(7);
        });
    }
}