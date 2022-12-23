using FlueFlame.AspNetCore.Grpc.Modules.BidirectionalStreaming;
using Grpc.Core;

namespace FlueFlame.AspNetCore.Grpc.Modules.ServerStreaming;

public class ServerStreamingRpcModule<TClient> : FlueFlameGrpcModuleBase<TClient> where TClient : ClientBase<TClient>
{
	internal ServerStreamingRpcModule(IFlueFlameGrpcHost host, TClient client) : base(host, client)
	{
	}
	
	internal ServerStreamingRpcModule(ServerStreamingRpcModule<TClient> module) : this(module.Host, module.Client)
	{
	}
	
	public ServerStreamingRpcModule<TClient, TRequest, TResponse> Call<TRequest, TResponse>(Func<TClient, AsyncServerStreamingCall<TResponse>> action) where TRequest : class where TResponse : class
	{
		var streamingCall = action(Client);
		return new ServerStreamingRpcModule<TClient, TRequest, TResponse>(this, streamingCall);
	}
}

public class ServerStreamingRpcModule<TClient, TRequest, TResponse> :  ServerStreamingRpcModule<TClient> where TClient : ClientBase<TClient> where TRequest : class where TResponse : class
{
	private BidirectionalResponseStreamRpcModule<TClient, TResponse, TRequest> ResponseStream { get; }

	internal ServerStreamingRpcModule(ServerStreamingRpcModule<TClient> module,
		AsyncServerStreamingCall<TResponse> streamingCall) : base(module)
	{
		ResponseStream =
			new BidirectionalResponseStreamRpcModule<TClient, TResponse, TRequest>(Host, Client, streamingCall.ResponseStream);
	}
}