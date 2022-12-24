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
	
	/// <summary>
	/// Assert Response.
	/// </summary>
	/// <param name="action">Action performed on response.</param>
	/// <returns></returns>
	
	public RpcResponseModule<TClient, TResponse> AssertThat(Action<TResponse> action)
	{
		action(Response);
		return this;
	}
	
	/// <summary>
	/// Assert thrown RpcException.
	/// </summary>
	/// <param name="action">Action performed on RpcException.</param>
	/// <returns></returns>
	public RpcResponseModule<TClient, TResponse> AssertError(Action<RpcException> action)
	{
		action(Exception);
		return this;
	}
	
	/// <summary>
	/// Assert Status of RpcException.
	/// </summary>
	/// <param name="action">Action performed on Status.</param>
	/// <returns></returns>
	public RpcResponseModule<TClient, TResponse> AssertStatus(Action<Status> action)
	{
		action(Exception.Status);
		return this;
	}
	
	/// <summary>
	/// Assert gRPC StatusCode of RpcException.
	/// </summary>
	/// <param name="statusCode">Result of a remote procedure call.</param>
	/// <returns></returns>
	public RpcResponseModule<TClient, TResponse> AssertStatusCode(StatusCode statusCode)
	{
		Exception.StatusCode.Should().Be(statusCode);
		return this;
	}
}