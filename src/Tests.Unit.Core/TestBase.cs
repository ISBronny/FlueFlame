using FlueFlame.Core;
using Moq;
using Tests.Unit.Core.Entities;

namespace Tests.Unit.Core;

public abstract class TestBase
{
	protected IFlueFlameHost FlueFlameHost;

	protected TestEntity TestEntity { get; } = TestEntityHelper.Random;

	public TestBase()
	{
		FlueFlameHost = new Mock<IFlueFlameHost>().Object;
	}
}