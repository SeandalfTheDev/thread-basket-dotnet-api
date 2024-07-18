using FakeItEasy;
using ThreadBasket.Domain.Contracts;

namespace ThreadBasket.WebApi.Tests.DmcThread;

public class TestFixture
{
    public readonly IDmcThreadRepository _repository = A.Fake<IDmcThreadRepository>();
}