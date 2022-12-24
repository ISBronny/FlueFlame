using FluentAssertions;
using Grpc.Core;

namespace FlueFlame.AspNetCore.Grpc.Modules.Common;

public class ResponseStreamRpcModule<TClient, TResponse, TReturn> : FlueFlameGrpcModuleBase<TClient> where TResponse : class where TClient : ClientBase<TClient>
where TReturn : ResponseStreamRpcModule<TClient, TResponse, TReturn>
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

	/// <summary>
	/// Provides low-level access to the reader stream.
	/// </summary>
	/// <param name="action">Action that works with the IAsyncStreamReader</param>
	/// <returns></returns>
	public TReturn WithStreamReader(Action<IAsyncStreamReader<TResponse>> action)
	{
		action(AsyncStreamReader);
		return (TReturn)this;
	}
	
	/// <summary>
	/// Provides low-level access to the reader stream.
	/// </summary>
	/// <param name="action">Action that works with the IAsyncStreamReader</param>
	/// <returns></returns>
	public TReturn WithStreamReader(Func<IAsyncStreamReader<TResponse>, Task> action)
	{
		action(AsyncStreamReader).GetAwaiter().GetResult();
		return (TReturn)this;
	}
	
	/// <summary>
	/// Advances the stream reader to the next element in the sequence.
	/// <para>
	/// Does not throw, but saves the RpcException.
	/// To assert an RpcException use <see cref="AssertError"/>
	/// </para>
	/// </summary>
	/// <returns></returns>
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
	
	/// <summary>
	/// Calls IAsyncStreamReader.MoveNext() and checks if the stream has finished.
	/// </summary>
	/// <returns></returns>
	public TReturn AssertEndOfStream()
	{
		try
		{
			AsyncStreamReader.MoveNext().GetAwaiter().GetResult().Should().BeFalse();
		}
		catch (RpcException e)
		{
			CurrentException = e;
		}
		
		return (TReturn)this;
	}
	
	/// <summary>
	/// Assert current object.
	/// </summary>
	/// <param name="action">Action performed on response.</param>
	/// <returns></returns>
	public TReturn AssertCurrent(Action<TResponse> action)
	{
		action(AsyncStreamReader.Current);
		return (TReturn)this;
	}
	
	/// <summary>
	/// Reads all data until the stream ends and applies the passed action on each response.
	/// </summary>
	/// <param name="action">Action performed on response.</param>
	/// <returns></returns>
	public TReturn AssertForEach(Action<TResponse> action)
	{
		return AssertForEach((response, _ ) => action(response));
	}
	
	/// <summary>
	/// Reads all data until the stream ends and applies the passed action on each response.
	/// </summary>
	/// <param name="action">Action performed on response. Uses indexing.</param>
	/// <returns></returns>
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
	
	/// <summary>
	/// Assert thrown RpcException.
	/// </summary>
	/// <param name="action">Action performed on RpcException.</param>
	/// <returns></returns>
	public TReturn AssertError(Action<RpcException> action)
	{
		action(CurrentException);
		return (TReturn)this;
	}
	
	/// <summary>
	/// Assert Status of RpcException.
	/// </summary>
	/// <param name="action">Action performed on Status.</param>
	/// <returns></returns>
	public TReturn AssertStatus(Action<Status> action)
	{
		action(CurrentException.Status);
		return (TReturn)this;
	}
	
	/// <summary>
	/// Assert gRPC StatusCode of RpcException.
	/// </summary>
	/// <param name="statusCode">Result of a remote procedure call.</param>
	/// <returns></returns>
	public TReturn AssertStatusCode(StatusCode statusCode)
	{
		CurrentException.Should().NotBeNull("No error happened");
		CurrentException.StatusCode.Should().Be(statusCode);
		return (TReturn)this;
	}
}