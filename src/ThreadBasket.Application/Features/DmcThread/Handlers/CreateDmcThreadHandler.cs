using ErrorOr;
using Mediator;
using ThreadBasket.Application.Extensions;
using ThreadBasket.Application.Features.DmcThread.Models;
using ThreadBasket.Application.Features.DmcThread.Validators;
using ThreadBasket.Domain.Contracts;

namespace ThreadBasket.Application.Features.DmcThread.Handlers;

public class CreateDmcThreadHandler(IDmcThreadRepository repository) : IRequestHandler<CreateDmcThreadRequest, ErrorOr<int?>>
{
    public async ValueTask<ErrorOr<int?>> Handle(CreateDmcThreadRequest request, CancellationToken ct)
    {
        var exists = await repository.ExistsAsync(request.Floss);

        if (exists)
        {
            return Error.Conflict(description: $"A Thread with Floss {request.Floss} already exists.");
        }
        
        var validation = await new CreateDmcThreadValidator().ValidateAsync(request, ct);

        if (!validation.IsValid)
        {
            return validation.ToErrorOrErrors().ToList();
        }
        
        var thread = new Domain.Entities.DmcThread
        {
            Name = request.Name,
            Floss = request.Floss,
            WebColor = request.WebColor ?? "#FFFFFF",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var threadId = await repository.AddThreadAsync(thread);

        return threadId;
    }
}