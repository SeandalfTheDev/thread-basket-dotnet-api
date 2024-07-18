using ErrorOr;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Execution;
using ThreadBasket.Application.Features.DmcThread.Handlers;
using ThreadBasket.Application.Features.DmcThread.Models;

namespace ThreadBasket.WebApi.Tests.DmcThread;

public class GetDmcThreadListTests : TestFixture
{
    [Fact]
    public async Task Handler_HasValidRequest_WillProduceNoErrors()
    {
        var threads = new List<Domain.Entities.DmcThread>()
        {
            new()
            {
                Id = 1,
                Name = "Red",
                Floss = "RED",
            },
            new()
            {
                Id = 2,
                Name = "Blue",
                Floss = "BLU"
            },
            new()
            {
                Id = 3,
                Name = "Green",
                Floss = "GRN"
            },
            new()
            {
                Id = 4,
                Name = "Yellow",
                Floss = "YLW"
            }
        };
        A.CallTo(() => _repository.GetThreadListAsync(A<int>.Ignored, A<int>.Ignored))
            .Returns(Task.FromResult<IEnumerable<Domain.Entities.DmcThread>>(threads));

        var request = new GetDmcThreadListRequest(1, 2);
        var handler = new GetDmcThreadListHandler(_repository);

        var response = await handler.Handle(request, default);

        using (new AssertionScope())
        {
            response.IsError.Should().BeFalse();
            response.Value.Should().NotBeNull();
            response.Value.Data.Should().HaveCount(threads.Count);
        }
    }

    [Fact]
    public async Task Handler_HasInvalidRequest_WillProduceValidationErrors()
    {
        var request = new GetDmcThreadListRequest(0, 0);
        var handler = new GetDmcThreadListHandler(_repository);
        
        var response = await handler.Handle(request, default);
        
        using (new AssertionScope())
        {
            response.IsError.Should().BeTrue();
            response.Errors.Should().HaveCount(2);
            response.Errors.All(x => x.Type == ErrorType.Validation).Should().BeTrue();
        }
    }
}