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

	public TReturn Write(TRequest request)
	{
		ClientStreamWriter.WriteAsync(request).Wait();
		return (TReturn)this;
	}

	public TReturn WriteMany(IEnumerable<TRequest> requests)
	{
		foreach (var r in requests)
			ClientStreamWriter.WriteAsync(r).Wait();
		return (TReturn)this;
	}
	
	public TReturn Complete()
	{
		ClientStreamWriter.CompleteAsync().Wait();
		return (TReturn)this;
	}
}
