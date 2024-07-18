using ErrorOr;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;
using ThreadBasket.Application.Features.DmcThread.Handlers;
using ThreadBasket.Application.Features.DmcThread.Models;
using ThreadBasket.Domain.Contracts;

namespace ThreadBasket.WebApi.Tests.DmcThread;

public class CreateHandlerTests
{
    private readonly IDmcThreadRepository _repository;

    public CreateHandlerTests()
    {
        _repository = A.Fake<IDmcThreadRepository>();
    }

    [Fact]
    public async Task Handler_HasValidRequest_WillProduceNoErrors()
    {
        var entityId = 1;
        A.CallTo(() => _repository.ExistsAsync(A<string>.Ignored)).Returns(Task.FromResult(false));
        A.CallTo(() => _repository.AddThreadAsync(A<Domain.Entities.DmcThread>.Ignored))
            .Returns(Task.FromResult(new int?(entityId)));
        
        
        var request = new CreateDmcThreadRequest("Red", "RED", "#FF0000");
        
        var handler = new CreateDmcThreadHandler(_repository);
        var response = await handler.Handle(request, CancellationToken.None);
        

        using (new AssertionScope())
        {
            response.IsError.Should().BeFalse();
            response.Value.HasValue.Should().BeTrue();
            response.Value.Should().Be(entityId);
            A.CallTo(() => _repository.ExistsAsync(request.Floss)).MustHaveHappened();
            A.CallTo(() => _repository.AddThreadAsync(A<Domain.Entities.DmcThread>.Ignored)).MustHaveHappened();
        }
    }

    [Fact]
    public async Task Handler_ThreadExists_WillProduceConflictError()
    {
        A.CallTo(() => _repository.ExistsAsync(A<string>.Ignored)).Returns(Task.FromResult(true));
        
        var request = new CreateDmcThreadRequest("Red", "RED", "#FF0000");
        
        var handler = new CreateDmcThreadHandler(_repository);
        var response = await handler.Handle(request, CancellationToken.None);

        using (new AssertionScope())
        {
            response.IsError.Should().BeTrue();
            response.Errors.Should().HaveCount(1);
            response.FirstError.Type.Should().Be(ErrorType.Conflict);
        }
    }

    [Fact]
    public async Task Handler_InvalidRequest_WillProduceValidationErrors()
    {
        A.CallTo(() => _repository.ExistsAsync(A<string>.Ignored)).Returns(Task.FromResult(false));
        var request = new CreateDmcThreadRequest("", "", "#FF000000");
        
        var handler = new CreateDmcThreadHandler(_repository);
        var response = await handler.Handle(request, CancellationToken.None);

        using (new AssertionScope())
        {
            response.IsError.Should().BeTrue();
            response.Errors.Should().HaveCount(3);
            response.Errors.All(e => e.Type == ErrorType.Validation).Should().BeTrue();
        }
    }
}