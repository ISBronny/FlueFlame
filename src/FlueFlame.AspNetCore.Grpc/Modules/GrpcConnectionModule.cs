using FlueFlame.AspNetCore.Grpc.Modules.BidirectionalStreaming;
using FlueFlame.AspNetCore.Grpc.Modules.ClientStreaming;
using FlueFlame.AspNetCore.Grpc.Modules.ServerStreaming;
using FlueFlame.AspNetCore.Grpc.Modules.Unary;
using Grpc.Core;

namespace FlueFlame.AspNetCore.Grpc.Modules;

public class GrpcConnectionModule<TClient> : FlueFlameGrpcModuleBase<TClient> where TClient : ClientBase<TClient>
{
	public UnaryRpcModule<TClient> Unary { get; }
	public ClientStreamingRpcModule<TClient> ClientStreaming { get; }
	public ServerStreamingRpcModule<TClient> ServerStreaming { get; }
	public BidirectionalStreamingRpcModule<TClient> Bidirectional { get; }

	public GrpcConnectionModule(IFlueFlameGrpcHost flameHost, TClient client) : base(flameHost, client)
	{
		Unary = new UnaryRpcModule<TClient>(flameHost, client);
		ClientStreaming = new ClientStreamingRpcModule<TClient>(flameHost, client);
		ServerStreaming = new ServerStreamingRpcModule<TClient>(flameHost, client);
		Bidirectional = new BidirectionalStreamingRpcModule<TClient>(flameHost, client);
	}

	// /// <summary>
	// /// Call Unary RPC
	// /// </summary>
	// /// <param name="procedure">A function that calls Unary RPC client method</param>
	// /// <param name="responseHandler">Response handling function</param>
	// /// <param name="exceptionHandler"></param>
	// /// <typeparam name="TResponse">Object type returned in response</typeparam>
	// /// <returns></returns>
	// public GrpcConnectionModule<TClient> UnaryRpc<TResponse>(
	// 	Func<TClient, TResponse> procedure)
	// {
	// 	var response = procedure(Client);
	// 	return this;
	// }

	/// <summary>
	/// Call Client streaming RPC
	/// </summary>
	/// <param name="procedure">A function that calls Client streaming RPC method</param>
	/// <param name="sendHandler">A function that implements sending a stream of objects of type TRequest</param>
	/// <param name="responseHandler">Response handling function</param>
	/// <typeparam name="TRequest">Object type sent in request</typeparam>
	/// <typeparam name="TResponse">Object type returned in response</typeparam>
	/// <returns></returns>
	// public GrpcConnectionModule<TClient> ClientStreaming<TRequest, TResponse>(
	// 	Func<TClient, AsyncClientStreamingCall<TRequest, TResponse>> procedure,
	// 	Func<AsyncClientStreamingCall<TRequest, TResponse>, Task> sendHandler,
	// 	Action<TResponse> responseHandler = null)
	// {
	// 	using var streamingCall = procedure(Client);
	// 	sendHandler(streamingCall);
	// 	streamingCall.RequestStream.CompleteAsync().Wait();
	// 	responseHandler?.Invoke(streamingCall.ResponseAsync.Result);
	// 	return this;
	// }
	
	/// <summary>
	/// Call Server streaming RPC
	/// </summary>
	/// <param name="procedure">A function that calls Server streaming RPC method</param>
	/// <param name="responseHandler">A function that handle a stream of incoming objects</param>
	/// <typeparam name="TResponse">Object type returned in response</typeparam>
	/// <returns></returns>
	// public GrpcConnectionModule<TClient> ServerStreaming<TResponse>(
	// 	Func<TClient, AsyncServerStreamingCall<TResponse>> procedure,
	// 	  responseHandler)
	// {
	// 	using var streamingCall = procedure(Client);
	// 	responseHandler(streamingCall).Wait();
	// 	return this;
	// }

	/// <summary>
	/// Call Bidirectional streaming RPC
	/// </summary>
	/// <param name="procedure"></param>
	/// <param name="senderAndReceiver">A function that implements sending objects and processing a stream of incoming objects.</param>
	/// <typeparam name="TRequest">Object type sent in request</typeparam>
	/// <typeparam name="TResponse">Object type returned in response</typeparam>
	/// <returns></returns>
	// public GrpcConnectionModule<TClient> BidirectionalStreaming<TRequest, TResponse>(
	// 	Func<TClient, AsyncDuplexStreamingCall<TRequest, TResponse>> procedure,
	// 	Func<AsyncDuplexStreamingCall<TRequest, TResponse>, Task> senderAndReceiver)
	// {
	// 	using var streamingCall = procedure(Client);
	// 	senderAndReceiver(streamingCall).ConfigureAwait(false).GetAwaiter().GetResult();
	// 	return this;
	// }
}