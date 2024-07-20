using ErrorOr;
using Mediator;
using ThreadBasket.Application.Extensions;
using ThreadBasket.Application.Features.DmcThread.Models;
using ThreadBasket.Application.Features.DmcThread.Validators;
using ThreadBasket.Domain.Contracts;

namespace ThreadBasket.Application.Features.DmcThread.Handlers;

public class DeleteDmcThreadHandler(IDmcThreadRepository repository) : IRequestHandler<DeleteDmcThreadRequest, ErrorOr<bool>>
{
    public async ValueTask<ErrorOr<bool>> Handle(DeleteDmcThreadRequest request, CancellationToken ct)
    {
        var validationResult = await new DeleteDmcThreadValidator().ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return validationResult.ToErrorOrErrors().ToList();
        }
        
        var exists = await repository.ExistsAsync(request.Id);
        if (!exists)
        {
            return Error.NotFound(description: $"Thread with id {request.Id} does not exist");
        }

        var deleted = await repository.DeleteThreadAsync(request.Id);
        return deleted;
    }
}