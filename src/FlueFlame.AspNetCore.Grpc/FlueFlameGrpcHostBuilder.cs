using FlueFlame.Core.Builders;
using Grpc.Net.Client;
using Microsoft.AspNetCore.TestHost;

namespace FlueFlame.AspNetCore.Grpc;

public class FlueFlameGrpcHostBuilder : FlueFlameHostBuilder<FlueFlameGrpcHostBuilder>
{
	private TestServer TestServer { get; }
	private GrpcChannelOptions GrpcChannelOptions { get; set; }

	public FlueFlameGrpcHostBuilder(TestServer testServer)
	{
		TestServer = testServer;
		HttpClient = TestServer.CreateClient();
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