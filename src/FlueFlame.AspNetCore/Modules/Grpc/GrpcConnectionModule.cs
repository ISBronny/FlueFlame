using System;
using System.Threading.Tasks;
using FlueFlame.AspNetCore.Common;
using Grpc.Core;

namespace FlueFlame.AspNetCore.Modules.Grpc;

public class GrpcConnectionModule<TClient> : FlueFlameModuleBase where TClient : ClientBase<TClient>
{
	private TClient Client { get; }
	public GrpcConnectionModule(TClient client, FlueFlameHost flameHost) : base(flameHost)
	{
		Client = client;
	}
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="procedure"></param>
	/// <param name="responseHandler"></param>
	/// <typeparam name="TResponse"></typeparam>
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
	/// 
	/// </summary>
	/// <param name="procedure"></param>
	/// <param name="sender"></param>
	/// <param name="responseHandler"></param>
	/// <typeparam name="TRequest"></typeparam>
	/// <typeparam name="TResponse"></typeparam>
	/// <returns></returns>
	public GrpcConnectionModule<TClient> Call<TRequest, TResponse>(
		Func<TClient, AsyncClientStreamingCall<TRequest, TResponse>> procedure,
		Func<AsyncClientStreamingCall<TRequest, TResponse>, Task> sender,
		Action<TResponse> responseHandler)
	{
		var streamingCall = procedure(Client);
		sender(streamingCall);
		responseHandler(streamingCall.ResponseAsync.Result);
		return this;
	}
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="procedure"></param>
	/// <param name="responseHandler"></param>
	/// <typeparam name="TResponse"></typeparam>
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
	/// 
	/// </summary>
	/// <param name="procedure"></param>
	/// <param name="senderAndReceiver"></param>
	/// <typeparam name="TRequest"></typeparam>
	/// <typeparam name="TResponse"></typeparam>
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