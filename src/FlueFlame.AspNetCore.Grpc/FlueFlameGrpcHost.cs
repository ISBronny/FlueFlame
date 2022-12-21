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

	/// <summary>
	/// Create gRPC connection
	/// </summary>
	/// <param name="options">Grpc Channel Options</param>
	/// <typeparam name="T">Type of gRPC Client</typeparam>
	/// <returns></returns>
	public GrpcConnectionModule<T> CreateConnection<T>(GrpcChannelOptions options = null) where T : ClientBase<T>
	{
		options ??= DefaultChannelOptions;
		options.HttpClient ??= HttpClient ?? TestServer.CreateClient();
		var channel = GrpcChannel.ForAddress(
			(options.Credentials == null ? "http" : "https") + $"://{TestServer.BaseAddress.Host}", options);
		var client = (T)Activator.CreateInstance(typeof(T), channel);
		return new GrpcConnectionModule<T>(this, client);
	}
	
	/// <summary>
	/// Sets JWT token header for authentication.
	/// </summary>
	/// <param name="token">JWT Token</param>
	/// <returns></returns>
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