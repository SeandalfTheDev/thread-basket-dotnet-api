using FluentValidation;
using ThreadBasket.Application.Features.DmcThread.Models;

namespace ThreadBasket.Application.Features.DmcThread.Validators;

public class GetDmcThreadListValidator : AbstractValidator<GetDmcThreadListRequest>
{
    public GetDmcThreadListValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0);

        RuleFor(x => x.Size)
            .GreaterThan(0);
    }
}