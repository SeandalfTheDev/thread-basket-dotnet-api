using FluentValidation;
using ThreadBasket.Application.Features.DmcThread.Models;

namespace ThreadBasket.Application.Features.DmcThread.Validators;

public class GetDmcThreadValidator : AbstractValidator<GetDmcThreadRequest>
{
    public GetDmcThreadValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}