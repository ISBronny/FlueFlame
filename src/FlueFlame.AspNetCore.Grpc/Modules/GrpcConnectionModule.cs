using FlueFlame.Core;
using Grpc.Core;

namespace FlueFlame.AspNetCore.Grpc.Modules;

public class GrpcConnectionModule<TClient> : FlueFlameModuleBase<IFlueFlameGrpcHost> where TClient : ClientBase<TClient>
{
	private TClient Client { get; }
	public GrpcConnectionModule(TClient client, IFlueFlameGrpcHost flameHost) : base(flameHost)
	{
		Client = client;
	}
	
	/// <summary>
	/// Call Unary RPC
	/// </summary>
	/// <param name="procedure">A function that calls Unary RPC client method</param>
	/// <param name="responseHandler">Response handling function</param>
	/// <typeparam name="TResponse">Object type returned in response</typeparam>
	/// <returns></returns>
	public GrpcConnectionModule<TClient> Call<TResponse>(
		Func<TClient, TResponse> procedure,
		Action<TResponse> responseHandler)
	{
		var response = procedure(Client);
		responseHandler(response);
		return this;
	}
	
	/// <summary>
	/// Call Client streaming RPC
	/// </summary>
	/// <param name="procedure">A function that calls Client streaming RPC method</param>
	/// <param name="sendHandler">A function that implements sending a stream of objects of type TRequest</param>
	/// <param name="responseHandler">Response handling function</param>
	/// <typeparam name="TRequest">Object type sent in request</typeparam>
	/// <typeparam name="TResponse">Object type returned in response</typeparam>
	/// <returns></returns>
	public GrpcConnectionModule<TClient> Call<TRequest, TResponse>(
		Func<TClient, AsyncClientStreamingCall<TRequest, TResponse>> procedure,
		Func<AsyncClientStreamingCall<TRequest, TResponse>, Task> sendHandler,
		Action<TResponse> responseHandler)
	{
		var streamingCall = procedure(Client);
		sendHandler(streamingCall);
		responseHandler(streamingCall.ResponseAsync.Result);
		return this;
	}
	
	/// <summary>
	/// Call Server streaming RPC
	/// </summary>
	/// <param name="procedure">A function that calls Server streaming RPC method</param>
	/// <param name="responseHandler">A function that handle a stream of incoming objects</param>
	/// <typeparam name="TResponse">Object type returned in response</typeparam>
	/// <returns></returns>
	public GrpcConnectionModule<TClient> Call<TResponse>(
		Func<TClient, AsyncServerStreamingCall<TResponse>> procedure,
		Func<AsyncServerStreamingCall<TResponse>,Task>  responseHandler)
	{
		var streamingCall = procedure(Client);
		responseHandler(streamingCall).Wait();
		return this;
	}

	/// <summary>
	/// Call Bidirectional streaming RPC
	/// </summary>
	/// <param name="procedure"></param>
	/// <param name="senderAndReceiver">A function that implements sending objects and processing a stream of incoming objects.</param>
	/// <typeparam name="TRequest">Object type sent in request</typeparam>
	/// <typeparam name="TResponse">Object type returned in response</typeparam>
	/// <returns></returns>
	public GrpcConnectionModule<TClient> Call<TRequest, TResponse>(
		Func<TClient, AsyncDuplexStreamingCall<TRequest, TResponse>> procedure,
		Func<AsyncDuplexStreamingCall<TRequest, TResponse>, Task> senderAndReceiver)
	{
		var streamingCall = procedure(Client);
		senderAndReceiver(streamingCall).Wait();
		return this;
	}
}