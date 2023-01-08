using FlueFlame.Core.Builders;
using Grpc.Net.Client;
using Microsoft.AspNetCore.TestHost;

namespace FlueFlame.AspNetCore.Grpc;

public class FlueFlameGrpcHostBuilder : FlueFlameHostBuilder<FlueFlameGrpcHostBuilder>
{
	private TestServer TestServer { get; }
	private GrpcChannelOptions GrpcChannelOptions { get; set; } = new();

	public FlueFlameGrpcHostBuilder(TestServer testServer, HttpClient httpClient = null)
	{
		TestServer = testServer;
		HttpClient = httpClient ?? TestServer.CreateClient();
		TestServer.CreateWebSocketClient();
	}

	public FlueFlameGrpcHostBuilder UseCustomGrpcChannelOptions(GrpcChannelOptions grpcChannelOptions)
	{
		GrpcChannelOptions = grpcChannelOptions;
		return this;
	}
	
	public IFlueFlameGrpcHost Build()
	{
		return new FlueFlameGrpcHost(TestServer, HttpClient, GrpcChannelOptions);
	}
}