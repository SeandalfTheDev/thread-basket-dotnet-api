using ErrorOr;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;
using ThreadBasket.Application.Features.DmcThread.Handlers;
using ThreadBasket.Application.Features.DmcThread.Models;

namespace ThreadBasket.WebApi.Tests.DmcThread;

public class UpdateDmcThreadTests : TestFixture
{
    [Fact]
    public async Task Handler_WhenRequestValid_WillReturnTrue()
    {
        A.CallTo(() => _repository.ExistsAsync(A<int>.Ignored))
            .Returns(true);
        
        A.CallTo(() => _repository.UpdateThreadAsync(A<Domain.Entities.DmcThread>.Ignored))
            .Returns(true);
        
        A.CallTo(() => _repository.GetThreadAsync(A<int>.Ignored))
            .Returns(new Domain.Entities.DmcThread()
            {
                Id = 1,
                Name = "Test",
                Floss = "TST",
                WebColor = "Test",
            });

        var request = new UpdateDmcThreadRequest(1, "Red", "Red", "#FF0000");
        var handler = new UpdateDmcThreadHandler(_repository);
        var response = await handler.Handle(request, CancellationToken.None);

        using (new AssertionScope())
        {
            response.IsError.Should().BeFalse();
            response.Value.Should().BeTrue();
        }
    }

    [Fact]
    public async Task Handler_WhenRequestIsInvalid_WillReturnValidationErrors()
    {
        A.CallTo(() => _repository.ExistsAsync(A<string>.Ignored)).Returns(Task.FromResult(true));
        A.CallTo(() => _repository.UpdateThreadAsync(A<Domain.Entities.DmcThread>.Ignored))
            .Returns(Task.FromResult(true));
        
        var request = new UpdateDmcThreadRequest(0, "", "", "");
        var handler = new UpdateDmcThreadHandler(_repository);

        var response = await handler.Handle(request, default);

        using (new AssertionScope())
        {
            response.IsError.Should().BeTrue();
            response.Errors.Should().HaveCount(5);
            response.Errors.All(x => x.Type == ErrorType.Validation).Should().BeTrue();
        }
    }

    [Fact]
    public async Task Handler_WhenThreadDoesNotExist_WillReturnNotFoundError()
    {
        A.CallTo(() => _repository.ExistsAsync(A<string>.Ignored)).Returns(Task.FromResult(false));
        A.CallTo(() => _repository.UpdateThreadAsync(A<Domain.Entities.DmcThread>.Ignored))
            .Returns(Task.FromResult(true));
        
        var request = new UpdateDmcThreadRequest(1, "Red", "Red", "#FF0000");
        var handler = new UpdateDmcThreadHandler(_repository);

        var response = await handler.Handle(request, default);
        
        using (new AssertionScope())
        {
            response.IsError.Should().BeTrue();
            response.Errors.Should().HaveCount(1);
            response.FirstError.Type.Should().Be(ErrorType.NotFound);
        }
    }
}