using System.Net.Sockets;
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
	
	/// <summary>
	/// Call Server streaming RPC
	/// </summary>
	/// <param name="action">A function that calls Server streaming RPC method</param>
	/// <typeparam name="TResponse">Object type returned in response</typeparam>
	/// <returns></returns>
	public ServerStreamingRpcModule<TClient, TResponse> Call<TResponse>(Func<TClient, AsyncServerStreamingCall<TResponse>> action) where TResponse : class
	{
		var streamingCall = action(Client);
		return new ServerStreamingRpcModule<TClient, TResponse>(this, streamingCall);
	}
}

public class ServerStreamingRpcModule<TClient, TResponse> :  ServerStreamingRpcModule<TClient> where TClient : ClientBase<TClient> where TResponse : class
{
	public ServerStreamingResponseRpcModule<TClient, TResponse> ResponseStream { get; }

	internal ServerStreamingRpcModule(ServerStreamingRpcModule<TClient> module,
		AsyncServerStreamingCall<TResponse> streamingCall) : base(module)
	{
		ResponseStream =
			new ServerStreamingResponseRpcModule<TClient, TResponse>(Host, Client, streamingCall.ResponseStream);
	}
}