using FluentAssertions;
using Grpc.Core;

namespace FlueFlame.AspNetCore.Grpc.Modules.Common;

public class ResponseStreamRpcModule<TClient, TResponse, TRequest> : FlueFlameGrpcModuleBase<TClient> where TResponse : class where TClient : ClientBase<TClient> where TRequest : class
{
	private IAsyncStreamReader<TResponse> AsyncStreamReader { get; }
	private RpcException CurrentException { get; set; }
	
	internal ResponseStreamRpcModule(IFlueFlameGrpcHost host,
		TClient client,
		IAsyncStreamReader<TResponse> asyncStreamReader
	) : base(host, client)
	{
		AsyncStreamReader = asyncStreamReader;
		
	}
	
	public ResponseStreamRpcModule<TClient, TResponse, TRequest> Next()
	{
		try
		{
			AsyncStreamReader.MoveNext().Wait();
		}
		catch (RpcException e)
		{
			CurrentException = e;
		}
		
		return this;
	}
	
	public ResponseStreamRpcModule<TClient, TResponse, TRequest> AssertCurrent(Action<TResponse> action)
	{
		action(AsyncStreamReader.Current);
		return this;
	}
	
	public ResponseStreamRpcModule<TClient, TResponse, TRequest> AssertError(Action<RpcException> action)
	{
		action(CurrentException);
		return this;
	}
	
	public ResponseStreamRpcModule<TClient, TResponse, TRequest> AssertStatus(Action<Status> action)
	{
		action(CurrentException.Status);
		return this;
	}
	
	public ResponseStreamRpcModule<TClient, TResponse, TRequest> AssertStatusCode(StatusCode statusCode)
	{
		CurrentException.StatusCode.Should().Be(statusCode);
		return this;
	}
}