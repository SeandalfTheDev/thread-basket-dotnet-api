using ErrorOr;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;
using ThreadBasket.Application.Features.DmcThread.Handlers;
using ThreadBasket.Application.Features.DmcThread.Models;

namespace ThreadBasket.WebApi.Tests.DmcThread;

public class DeleteDmcThreadTests : TestFixture
{
    [Fact]
    public async Task Handler_WhenRequestIsValid_WillReturnNoErrors()
    {
        A.CallTo(() => _repository.ExistsAsync(A<int>.Ignored)).Returns(true);
        A.CallTo(() => _repository.DeleteThreadAsync(A<int>.Ignored)).Returns(true);
        
        var request = new DeleteDmcThreadRequest(1);
        var handler = new DeleteDmcThreadHandler(_repository);

        var response = await handler.Handle(request, default);

        using (new AssertionScope())
        {
            response.IsError.Should().BeFalse();
            response.Value.Should().BeTrue();
        }
    }

    [Fact]
    public async Task Handler_WhenRequestIsInvalid_WillReturnValidationErrors()
    {
        A.CallTo(() => _repository.ExistsAsync(A<int>.Ignored)).Returns(true);
        A.CallTo(() => _repository.DeleteThreadAsync(A<int>.Ignored)).Returns(true);
        
        var request = new DeleteDmcThreadRequest(0);
        var handler = new DeleteDmcThreadHandler(_repository);

        var response = await handler.Handle(request, default);

        using (new AssertionScope())
        {
            response.IsError.Should().BeTrue();
            response.Errors.Should().HaveCount(2);
            response.Errors.All(x => x.Type == ErrorType.Validation).Should().BeTrue();
        }
    }

    [Fact]
    public async Task Handler_WhenThreadDoesNotExist_WillReturnNotFoundError()
    {
        A.CallTo(() => _repository.ExistsAsync(A<int>.Ignored)).Returns(false);
        A.CallTo(() => _repository.DeleteThreadAsync(A<int>.Ignored)).Returns(true);
        
        var request = new DeleteDmcThreadRequest(1);
        var handler = new DeleteDmcThreadHandler(_repository);

        var response = await handler.Handle(request, default);

        using (new AssertionScope())
        {
            response.IsError.Should().BeTrue();
            response.FirstError.Type.Should().Be(ErrorType.NotFound);
        }
    }
}