using Grpc.Core;

namespace FlueFlame.AspNetCore.Grpc.Modules.BidirectionalStreaming;

public class BidirectionalStreamingRpcModule<TClient> : FlueFlameGrpcModuleBase<TClient> where TClient : ClientBase<TClient>
{
	internal BidirectionalStreamingRpcModule(IFlueFlameGrpcHost host, TClient client) : base(host, client)
	{
	}
	
	internal BidirectionalStreamingRpcModule(BidirectionalStreamingRpcModule<TClient> module) : base(module.Host, module.Client)
	{
	}
	
	/// <summary>
	/// Call Bidirectional streaming RPC
	/// </summary>
	/// <param name="action">A function that calls Bidirectional streaming RPC method</param>
	/// <typeparam name="TResponse">Object type returned in response</typeparam>
	/// <typeparam name="TRequest">Object type sent in request</typeparam>
	/// <returns></returns>
	public BidirectionalStreamingRpcModule<TClient, TRequest, TResponse> Call<TRequest, TResponse>(Func<TClient, AsyncDuplexStreamingCall<TRequest, TResponse>> action) where TRequest : class where TResponse : class
	{
		var call = action(Client);
		return new BidirectionalStreamingRpcModule<TClient, TRequest, TResponse>(this, call);
	}
}

public class BidirectionalStreamingRpcModule<TClient, TRequest, TResponse> : BidirectionalStreamingRpcModule<TClient> where TClient : ClientBase<TClient> where TResponse : class where TRequest : class
{
	private AsyncDuplexStreamingCall<TRequest, TResponse> Call { get; }

	internal BidirectionalStreamingRpcModule(BidirectionalStreamingRpcModule<TClient> module, AsyncDuplexStreamingCall<TRequest, TResponse> call) : base(module)
	{
		Call = call;

		RequestStream = new BidirectionalRequestStreamRpcModule<TClient, TRequest, TResponse>(Host, Client, call.RequestStream);
		ResponseStream = new BidirectionalResponseStreamRpcModule<TClient, TResponse, TRequest>(Host, Client, call.ResponseStream);

		RequestStream.ResponseStream = ResponseStream;
		ResponseStream.RequestStream = RequestStream;
	}

	public BidirectionalRequestStreamRpcModule<TClient, TRequest, TResponse> RequestStream { get; }
	public BidirectionalResponseStreamRpcModule<TClient, TResponse, TRequest> ResponseStream { get; }
}