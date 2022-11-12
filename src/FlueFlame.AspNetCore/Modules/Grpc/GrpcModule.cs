using System;
using System.Threading.Tasks;
using FlueFlame.AspNetCore.Common;
using Grpc.Core;
using Grpc.Net.Client;

namespace FlueFlame.AspNetCore.Modules.Grpc;

public sealed class GrpcModule : FlueFlameModuleBase
{
	private GrpcChannelOptions ChannelOptions { get; }
	internal GrpcModule(IFlueFlameHost application) : base(application)
	{
		ChannelOptions = new GrpcChannelOptions();
	}

	/// <summary>
	/// Create gRPC connection
	/// </summary>
	/// <param name="options">Grpc Channel Options</param>
	/// <typeparam name="T">Type of gRPC Client</typeparam>
	/// <returns></returns>
	public GrpcConnectionModule<T> CreateConnection<T>(GrpcChannelOptions options = null) where T : ClientBase<T>
	{
		options ??= ChannelOptions;
		options.HttpClient ??= Application.TestServer.CreateClient();
		var channel = GrpcChannel.ForAddress(
			(options.Credentials == null ? "http" : "https") + $"://{Application.TestServer.BaseAddress.Host}", options);
		var client = (T)Activator.CreateInstance(typeof(T), channel);
		return new GrpcConnectionModule<T>(client, Application);
	}
	
	/// <summary>
	/// Sets JWT token header for authentication.
	/// </summary>
	/// <param name="token">JWT Token</param>
	/// <returns></returns>
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