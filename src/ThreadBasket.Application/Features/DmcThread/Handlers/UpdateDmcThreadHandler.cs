using ErrorOr;
using Mediator;
using ThreadBasket.Application.Extensions;
using ThreadBasket.Application.Features.DmcThread.Models;
using ThreadBasket.Application.Features.DmcThread.Validators;
using ThreadBasket.Domain.Contracts;

namespace ThreadBasket.Application.Features.DmcThread.Handlers;

public class UpdateDmcThreadHandler(IDmcThreadRepository repository)
    : IRequestHandler<UpdateDmcThreadRequest, ErrorOr<bool>>
{
    public async ValueTask<ErrorOr<bool>> Handle(UpdateDmcThreadRequest request, CancellationToken ct)
    {
        var validationResult = await new UpdateDmcThreadValidator().ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return validationResult.ToErrorOrErrors().ToList();
        }

        var exists = await repository.ExistsAsync(request.Id);

        if (!exists)
        {
            return Error.NotFound(description: $"Thread with {request.Id} was not found.");
        }

        var entity = await repository.GetThreadAsync(request.Id);

        entity.Name = request.Name;
        entity.Floss = request.Floss;
        entity.WebColor = request.WebColor;
        entity.UpdatedAt = DateTime.Now;

        var updated = await repository.UpdateThreadAsync(entity);

        return updated;
    }
}