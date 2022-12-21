using FluentAssertions;
using Grpc.Core;

namespace FlueFlame.AspNetCore.Grpc.Modules.Common;

public class RpcResponseModule<TClient, TResponse> : FlueFlameGrpcModuleBase<TClient> where TClient : ClientBase<TClient>
{
	private TResponse Response { get; }
	private RpcException Exception { get; }

	internal RpcResponseModule(IFlueFlameGrpcHost host, TClient client, TResponse response) : base(host, client)
	{
		Response = response;
	}
	
	internal RpcResponseModule(IFlueFlameGrpcHost host, TClient client, RpcException exception) : base(host, client)
	{
		Exception = exception;
	}
	
	public RpcResponseModule<TClient, TResponse> AssertThat(Action<TResponse> action)
	{
		action(Response);
		return this;
	}
	
	public RpcResponseModule<TClient, TResponse> AssertError(Action<RpcException> action)
	{
		action(Exception);
		return this;
	}
	
	public RpcResponseModule<TClient, TResponse> AssertStatusCode(StatusCode statusCode)
	{
		Exception.StatusCode.Should().Be(statusCode);
		return this;
	}
}