using FlueFlame.AspNetCore;
using Moq;
using Testing.Tests.UnitTests.Entities;

namespace Testing.Tests.UnitTests;

public abstract class TestBase
{
	protected IFlueFlameHost FlueFlameHost;

	protected TestEntity TestEntity { get; } = TestEntityHelper.Random;

	public TestBase()
	{
		var hostMock = new Mock<IFlueFlameHost>();
		FlueFlameHost = hostMock.Object;
	}
}