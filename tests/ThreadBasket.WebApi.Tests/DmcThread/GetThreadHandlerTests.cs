using ErrorOr;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;
using ThreadBasket.Application.Features.DmcThread.Handlers;
using ThreadBasket.Application.Features.DmcThread.Models;

namespace ThreadBasket.WebApi.Tests.DmcThread;

public class GetThreadHandlerTests : TestFixture
{
    [Fact]
    public async Task Handler_WhenRequestIsValid_ReturnsThread()
    {
        var entity = new Domain.Entities.DmcThread
        {
            Id = 1,
            Name = "Thread 1",
            Floss = "Floss 1",
        };

        A.CallTo(() => _repository.GetThreadAsync(A<int>.Ignored))!
            .Returns(Task.FromResult(entity));
        
        var request = new GetDmcThreadRequest(entity.Id);
        var handler = new GetDmcThreadHandler(_repository);

        var response = await handler.Handle(request, default);

        using (new AssertionScope())
        {
            response.IsError.Should().BeFalse();
            response.Value.Should().NotBeNull();
            response.Value.Name.Should().Be(entity.Name);
            response.Value.Floss.Should().Be(entity.Floss);
        }
    }

    [Fact]
    public async Task Handler_WhenRequestIsInvalid_ReturnsValidationError()
    {
        var request = new GetDmcThreadRequest(0);
        var handler = new GetDmcThreadHandler(_repository);

        var response = await handler.Handle(request, default);

        using (new AssertionScope())
        {
            response.IsError.Should().BeTrue();
            response.FirstError.Type.Should().Be(ErrorType.Validation);
        }
    }

    [Fact]
    public async Task Handler_WhenThreadNotFound_ReturnsNotFound()
    {
        A.CallTo(() => _repository.GetThreadAsync(A<int>.Ignored))
            .Returns(Task.FromResult<Domain.Entities.DmcThread?>(null));
        
        var request = new GetDmcThreadRequest(1);
        var handler = new GetDmcThreadHandler(_repository);

        var response = await handler.Handle(request, default);

        using (new AssertionScope())
        {
            response.IsError.Should().BeTrue();
            response.FirstError.Type.Should().Be(ErrorType.NotFound);
        }
    }
}