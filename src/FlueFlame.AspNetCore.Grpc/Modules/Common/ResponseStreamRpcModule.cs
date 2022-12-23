using FlueFlame.AspNetCore.Grpc.Extensions;
using FluentAssertions;
using Grpc.Core;

namespace FlueFlame.AspNetCore.Grpc.Modules.Common;

public class ResponseStreamRpcModule<TClient, TResponse, TRequest, TReturn> : FlueFlameGrpcModuleBase<TClient> where TResponse : class where TClient : ClientBase<TClient> where TRequest : class
where TReturn : ResponseStreamRpcModule<TClient, TResponse, TRequest, TReturn>
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
	
	public TReturn Next()
	{
		try
		{
			AsyncStreamReader.MoveNext().GetAwaiter().GetResult();
		}
		catch (RpcException e)
		{
			CurrentException = e;
		}


		return (TReturn)this;
	}
	
	public TReturn AssertCurrent(Action<TResponse> action)
	{
		action(AsyncStreamReader.Current);
		return (TReturn)this;
	}
	
	public TReturn AssertForEach(Action<TResponse> action)
	{
		return AssertForEach((response, _ ) => action(response));
	}
	
	public TReturn AssertForEach(Action<TResponse, int> action)
	{

		Task.Run(async () =>
		{
			var i = 0;
			await foreach (var response in AsyncStreamReader.ReadAllAsync())
			{
				action(response, i++);
			}
		}).GetAwaiter().GetResult();
		
		return (TReturn)this;
	}
	
	public TReturn AssertError(Action<RpcException> action)
	{
		action(CurrentException);
		return (TReturn)this;
	}
	
	public TReturn AssertStatus(Action<Status> action)
	{
		action(CurrentException.Status);
		return (TReturn)this;
	}
	
	public TReturn AssertStatusCode(StatusCode statusCode)
	{
		CurrentException.Should().NotBeNull("No error happened");
		CurrentException.StatusCode.Should().Be(statusCode);
		return (TReturn)this;
	}
}