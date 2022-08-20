using System;
using System.Threading.Tasks;
using FlueFlame.AspNetCore.Common;
using Grpc.Core;
using Grpc.Net.Client;

namespace FlueFlame.AspNetCore.Modules.Grpc;

public sealed class GrpcModule : AspNetModuleBase
{
	private GrpcChannelOptions ChannelOptions { get; }
	internal GrpcModule(FlueFlameHost application) : base(application)
	{
		ChannelOptions = new GrpcChannelOptions();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="options"></param>
	/// <typeparam name="T"></typeparam>
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
	/// <param name="token"></param>
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