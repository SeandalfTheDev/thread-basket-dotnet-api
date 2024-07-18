using ErrorOr;
using Mediator;
using ThreadBasket.Application.Extensions;
using ThreadBasket.Application.Features.DmcThread.Models;
using ThreadBasket.Application.Features.DmcThread.Validators;
using ThreadBasket.Domain.Contracts;

namespace ThreadBasket.Application.Features.DmcThread.Handlers;

public class GetDmcThreadHandler(IDmcThreadRepository repository)
    : IRequestHandler<GetDmcThreadRequest, ErrorOr<Domain.Entities.DmcThread?>>
{
    public async ValueTask<ErrorOr<Domain.Entities.DmcThread?>> Handle(GetDmcThreadRequest request, CancellationToken ct)
    {
        var validation = await new GetDmcThreadValidator().ValidateAsync(request, ct);

        if (!validation.IsValid)
        {
            return validation.ToErrorOrErrors().ToList();
        }

        var thread = await repository.GetThreadAsync(request.Id);

        if (thread == null)
        {
            return Error.NotFound($"Thread with id: {request.Id} was not found");
        }

        return thread;
    }
}