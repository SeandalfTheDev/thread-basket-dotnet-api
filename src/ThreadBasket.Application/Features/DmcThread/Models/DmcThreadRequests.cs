using ErrorOr;
using Mediator;

namespace ThreadBasket.Application.Features.DmcThread.Models;

public record GetDmcThreadRequest(int Id) : IRequest<ErrorOr<Domain.Entities.DmcThread?>>;

public record GetDmcThreadListRequest(int Page = 1, int Size = 20) : IRequest<ErrorOr<PagedList<Domain.Entities.DmcThread>>>;

public record CreateDmcThreadRequest(string Name, string Floss, string? WebColor) : IRequest<ErrorOr<int?>>;

public record UpdateDmcThreadRequest(int Id, string Name, string Floss, string WebColor) : IRequest<ErrorOr<bool>>;

public record DeleteDmcThreadRequest(int Id) : IRequest<ErrorOr<bool>>;