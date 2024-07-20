using FluentValidation;
using ThreadBasket.Application.Features.DmcThread.Models;

namespace ThreadBasket.Application.Features.DmcThread.Validators;

public class UpdateDmcThreadValidator : AbstractValidator<UpdateDmcThreadRequest>
{
    public UpdateDmcThreadValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .GreaterThan(0);
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Floss)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.WebColor)
            .NotEmpty()
            .MaximumLength(7);
    }
}