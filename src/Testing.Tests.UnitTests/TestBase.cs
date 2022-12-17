using FlueFlame.AspNetCore.Grpc;
using FlueFlame.AspNetCore.SignalR.Host;
using FlueFlame.Core;
using FlueFlame.Http.Host;
using Moq;
using Testing.Tests.UnitTests.Entities;

namespace Testing.Tests.UnitTests;

public abstract class TestBase
{
	protected IFlueFlameHttpHost FlueFlameHttpHost;
	protected IFlueFlameGrpcHost FlueFlameGrpcHost;
	protected IFlueFlameSignalRHost FlueFlameSignalRHost;

	protected TestEntity TestEntity { get; } = TestEntityHelper.Random;

	public TestBase()
	{
		FlueFlameHttpHost = new Mock<IFlueFlameHttpHost>().Object;
		FlueFlameGrpcHost = new Mock<IFlueFlameGrpcHost>().Object;
		FlueFlameSignalRHost = new Mock<IFlueFlameSignalRHost>().Object;
	}
}