using FlueFlame.AspNetCore.Grpc.Modules;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.TestHost;

namespace FlueFlame.AspNetCore.Grpc;

public class FlueFlameGrpcHost : IFlueFlameGrpcHost
{
	private GrpcChannelOptions DefaultChannelOptions { get; } = new();
	private TestServer TestServer { get; }
	private HttpClient HttpClient { get; }
	
	public FlueFlameGrpcHost(TestServer testServer, HttpClient httpClient)
	{
		TestServer = testServer;
		HttpClient = httpClient;
	}
	
	public FlueFlameGrpcHost(TestServer testServer, HttpClient httpClient, GrpcChannelOptions grpcDefaultChannelOptions) : this(testServer, httpClient)
	{
		DefaultChannelOptions = grpcDefaultChannelOptions;
	}

	/// <inheritdoc />
	public GrpcFacadeModule<T> CreateConnection<T>()
		where T : ClientBase<T>
	{
		var options = DefaultChannelOptions;
		options.HttpClient ??= HttpClient ?? TestServer.CreateClient();
		return CreateConnection<T>(options);

	}
	
	/// <inheritdoc />
	public GrpcFacadeModule<T> CreateConnection<T>(GrpcChannelOptions options)
		where T : ClientBase<T>
	{
		options.HttpClient ??= HttpClient ?? TestServer.CreateClient();
		var grpcChannel = GrpcChannel.ForAddress(
			(options.Credentials == null ? "http" : "https") + $"://{TestServer.BaseAddress.Host}", options);
		return CreateConnection<T>(grpcChannel);
	}
	
	/// <inheritdoc />
	public GrpcFacadeModule<T> CreateConnection<T>(GrpcChannel grpcChannel)
		where T : ClientBase<T>
	{
		var client = (T)Activator.CreateInstance(typeof(T), grpcChannel);
		return new GrpcFacadeModule<T>(this, client);
	}
	
	/// <inheritdoc />
	public FlueFlameGrpcHost UseJwtToken(string token)
	{
		DefaultChannelOptions.Credentials = ChannelCredentials.Create(new SslCredentials(),
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