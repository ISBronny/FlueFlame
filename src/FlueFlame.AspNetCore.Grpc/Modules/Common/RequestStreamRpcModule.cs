using Grpc.Core;

namespace FlueFlame.AspNetCore.Grpc.Modules.Common;

public class RequestStreamRpcModule<TClient, TRequest, TResponse, TReturn> : FlueFlameGrpcModuleBase<TClient>
	where TClient : ClientBase<TClient> where TRequest : class where TResponse : class
	where TReturn : RequestStreamRpcModule<TClient, TRequest, TResponse, TReturn>
{
	protected IClientStreamWriter<TRequest> ClientStreamWriter { get; }
	
	internal RequestStreamRpcModule(IFlueFlameGrpcHost host,
		TClient client,
		IClientStreamWriter<TRequest> clientStreamWriter) : base(host, client)
	{
		ClientStreamWriter = clientStreamWriter;
	}

	/// <summary>
	/// Provides low-level access to the writing stream.
	/// </summary>
	/// <param name="action">Action that works with the IClientStreamWriter</param>
	/// <returns></returns>
	public TReturn WithStreamWriter(Action<IClientStreamWriter<TRequest>> action)
	{
		action(ClientStreamWriter);
		return (TReturn)this;
	}
	
	/// <summary>
	/// Provides low-level access to the writing stream.
	/// </summary>
	/// <param name="action">Action that works with the IClientStreamWriter</param>
	/// <returns></returns>
	public TReturn WithStreamWriter(Func<IClientStreamWriter<TRequest>, Task> action)
	{
		action(ClientStreamWriter).GetAwaiter().GetResult();
		return (TReturn)this;
	}

	/// <summary>
	/// Writes a message.
	/// </summary>
	/// <param name="request">The message to be written. Cannot be null.</param>
	/// <returns></returns>
	public TReturn Write(TRequest request)
	{
		ClientStreamWriter.WriteAsync(request).Wait();
		return (TReturn)this;
	}

	/// <summary>
	/// Writes a set of messages in order.
	/// </summary>
	/// <param name="requests">The message to be written. Cannot be null.</param>
	/// <returns></returns>
	public TReturn WriteMany(IEnumerable<TRequest> requests)
	{
		foreach (var r in requests)
			ClientStreamWriter.WriteAsync(r).Wait();
		return (TReturn)this;
	}
	
	/// <summary>
	/// Completes/closes the stream.
	/// </summary>
	/// <returns></returns>
	public TReturn Complete()
	{
		ClientStreamWriter.CompleteAsync().Wait();
		return (TReturn)this;
	}
}
