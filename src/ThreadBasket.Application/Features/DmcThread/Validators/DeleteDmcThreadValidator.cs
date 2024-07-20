using FluentValidation;
using ThreadBasket.Application.Features.DmcThread.Models;

namespace ThreadBasket.Application.Features.DmcThread.Validators;

public class DeleteDmcThreadValidator : AbstractValidator<DeleteDmcThreadRequest>
{
    public DeleteDmcThreadValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id cannot be empty")
            .GreaterThan(0);
    }
}