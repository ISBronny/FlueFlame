using FlueFlame.AspNetCore.Services.SignalR;
using FluentAssertions;

namespace Testing.Tests.UnitTests.SignalR;

public class HubConnectionMethodsObserverTests : TestBase
{
	private HubConnectionMethodsObserver Observer { get; } = new();
	private string MethodName => "TestMethod";
	private string MethodName2 => "TestMethod2";
	private TimeSpan Timeout => TimeSpan.FromMilliseconds(20);

	[Fact]
	public void WaitForMethodCall_MethodCalled_ShouldNotThrow()
	{
		Observer.NotifyResponse(MethodName);
		FluentActions.Invoking(() => Observer.WaitForMethodCall(MethodName, Timeout))
			.Should().NotThrow();
	}
	
	[Fact]
	public void WaitForMethodCall_SameMethodCalledAndWaitedTwoTimes_ShouldNotThrow()
	{
		Observer.NotifyResponse(MethodName);
		FluentActions.Invoking(() => Observer.WaitForMethodCall(MethodName, Timeout))
			.Should().NotThrow();
		Observer.NotifyResponse(MethodName);
		FluentActions.Invoking(() => Observer.WaitForMethodCall(MethodName, Timeout))
			.Should().NotThrow();
	}
	
	[Fact]
	public void WaitForMethodCall_DifferentMethodsCalledAndWaited_ShouldNotThrow()
	{
		Observer.NotifyResponse(MethodName);
		Observer.NotifyResponse(MethodName2);
		FluentActions.Invoking(() => Observer.WaitForMethodCall(MethodName, Timeout))
			.Should().NotThrow();
		FluentActions.Invoking(() => Observer.WaitForMethodCall(MethodName2, Timeout))
			.Should().NotThrow();
	}
	
	[Fact]
	public void WaitForMethodCall_MethodNotCalled_ShouldThrow()
	{
		FluentActions.Invoking(() => Observer.WaitForMethodCall(MethodName, Timeout))
			.Should().Throw<InvalidOperationException>();
	}
	
	[Fact]
	public void WaitForMethodCall_MethodAlreadyWaited_ShouldThrow()
	{
		Observer.NotifyResponse(MethodName);
		Observer.WaitForMethodCall(MethodName);
		FluentActions.Invoking(() => Observer.WaitForMethodCall(MethodName, Timeout))
			.Should().Throw<InvalidOperationException>();
	}
}