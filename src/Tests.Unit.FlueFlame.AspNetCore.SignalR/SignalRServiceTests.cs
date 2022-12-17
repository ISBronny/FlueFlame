using System.Net;
using FlueFlame.AspNetCore.SignalR.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Tests.Unit.AspNetCore.SignalR;

public class SignalRServiceTests
{
	private SignalRService SignalRService { get; } = new();

	private HubConnection HubConnectionStub { get; } = new(
		new Mock<IConnectionFactory>().Object,
		new Mock<IHubProtocol>().Object,
		new Mock<EndPoint>().Object,
		new Mock<IServiceProvider>().Object,
		NullLoggerFactory.Instance
	);

	[Fact]
	public void RegisterConnection_WithoutSpecifiedId_ReturnsGeneratedId()
	{
		var id = SignalRService.RegisterConnection(HubConnectionStub);
		Assert.NotNull(id);
	}
	
	[Fact]
	public void RegisterConnection_WithSpecifiedId_ReturnsId()
	{
		var expectedId = Guid.NewGuid();
		var id = SignalRService.RegisterConnection(HubConnectionStub, expectedId);
		Assert.Equal(expectedId, id);
	}
	
	[Fact]
	public void RegisterConnection_DuplicateId_Throws()
	{
		var id = SignalRService.RegisterConnection(HubConnectionStub);
		FluentActions.Invoking(() => SignalRService.RegisterConnection(HubConnectionStub, id))
			.Should().Throw<ArgumentException>();
	}
	
	[Fact]
	public void IsConnectionExists_Exists_ReturnsTrue()
	{
		var id = SignalRService.RegisterConnection(HubConnectionStub);
		Assert.True(SignalRService.IsConnectionExists(id));
	}

	[Fact]
	public void GetHubConnectionById_Exists_ReturnsHub()
	{
		var id = SignalRService.RegisterConnection(HubConnectionStub);
		Assert.StrictEqual(HubConnectionStub, SignalRService.GetHubConnectionById(id));
	}
	
	[Fact]
	public void GetObserverById_Exists_ReturnsHubObserver()
	{
		var id = SignalRService.RegisterConnection(HubConnectionStub);
		Assert.NotNull(SignalRService.GetHubConnectionObserverById(id));
	}
	
	[Fact]
	public void IsConnectionExists_NotExists_ReturnsFalse()
	{
		Assert.False(SignalRService.IsConnectionExists("SomeId"));
	}
}