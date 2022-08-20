using System;
using System.Threading.Tasks;
using FlueFlame.AspNetCore.Common;
using Grpc.Core;
using Grpc.Net.Client;

namespace FlueFlame.AspNetCore.Modules.Grpc;

public class GrpcModule : AspNetModuleBase
{
	protected GrpcChannelOptions ChannelOptions { get; }
	public GrpcModule(FlueFlameHost application) : base(application)
	{
		ChannelOptions = new GrpcChannelOptions();
	}

	public GrpcConnectionModule<T> CreateConnection<T>(GrpcChannelOptions options = null) where T : ClientBase<T>
	{
		options ??= ChannelOptions;
		options.HttpClient ??= Application.TestServer.CreateClient();
		var channel = GrpcChannel.ForAddress(
			(options.Credentials == null ? "http" : "https") + $"://{Application.TestServer.BaseAddress.Host}", options);
		var client = (T)Activator.CreateInstance(typeof(T), channel);
		return new GrpcConnectionModule<T>(client, Application);
	}

	public GrpcModule UseJwtToken(string token)
	{
		ChannelOptions.Credentials = ChannelCredentials.Create(new SslCredentials(),
			CallCredentials.FromInterceptor((_, metadata) =>
			{
				if (!string.IsNullOrEmpty(token))
				{
					metadata.Add("Authorization", $"Bearer {token}");
				}

				return Task.CompletedTask;
			}));
		return this;
	}
}

public class GrpcConnectionModule<TClient> : AspNetModuleBase where TClient : ClientBase<TClient>
{
	private TClient Client { get; }
	public GrpcConnectionModule(TClient client, FlueFlameHost flameHost) : base(flameHost)
	{
		Client = client;
	}
	
	public GrpcConnectionModule<TClient> Call<TResponse>(
		Func<TClient, TResponse> procedure,
		Action<TResponse> responseHandler)
	{
		var response = procedure(Client);
		responseHandler(response);
		return this;
	}
	
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
	
	public GrpcConnectionModule<TClient> Call<TResponse>(
		Func<TClient, AsyncServerStreamingCall<TResponse>> procedure,
		Func<AsyncServerStreamingCall<TResponse>,Task>  responseHandler)
	{
		var streamingCall = procedure(Client);
		responseHandler(streamingCall).Wait();
		return this;
	}

	public GrpcConnectionModule<TClient> Call<TRequest, TResponse>(
		Func<TClient, AsyncDuplexStreamingCall<TRequest, TResponse>> procedure,
		Func<AsyncDuplexStreamingCall<TRequest, TResponse>, Task> senderAndReceiver)
	{
		var streamingCall = procedure(Client);
		senderAndReceiver(streamingCall).Wait();
		return this;
	}
}