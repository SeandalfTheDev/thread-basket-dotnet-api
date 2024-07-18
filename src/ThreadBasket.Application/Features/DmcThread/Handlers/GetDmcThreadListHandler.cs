using ErrorOr;
using Mediator;
using ThreadBasket.Application.Extensions;
using ThreadBasket.Application.Features.DmcThread.Models;
using ThreadBasket.Application.Features.DmcThread.Validators;
using ThreadBasket.Domain.Contracts;

namespace ThreadBasket.Application.Features.DmcThread.Handlers;

public class GetDmcThreadListHandler(IDmcThreadRepository repository) 
    : IRequestHandler<GetDmcThreadListRequest, ErrorOr<PagedList<Domain.Entities.DmcThread>>>
{
    public async ValueTask<ErrorOr<PagedList<Domain.Entities.DmcThread>>> Handle(GetDmcThreadListRequest request, 
        CancellationToken ct)
    {
        var validation = await new GetDmcThreadListValidator().ValidateAsync(request, ct);

        if (!validation.IsValid)
        {
            return validation.ToErrorOrErrors().ToList();
        }

        var threadList = await repository.GetThreadListAsync(request.Page, request.Size);
        var totalCount = await repository.CountAsync();
        var pageCount = (int)Math.Ceiling((double)totalCount / request.Size);
        var hasNextPage = request.Page < pageCount;
        var hasPreviousPage = request.Page > 1;

        var pagedList = new PagedList<Domain.Entities.DmcThread>(threadList, request.Page, request.Size, pageCount,
            totalCount, hasNextPage, hasPreviousPage);

        return pagedList;
    }
}