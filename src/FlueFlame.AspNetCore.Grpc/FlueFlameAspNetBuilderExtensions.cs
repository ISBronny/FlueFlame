using Grpc.Net.Client;

namespace FlueFlame.AspNetCore.Grpc;

public static class FlueFlameAspNetBuilderExtensions
{
	public static IFlueFlameGrpcHost BuildGrpcHost(this FlueFlameAspNetBuilder builder)
	{
		return new FlueFlameGrpcHost(builder.TestServer);
	}
	
	public static IFlueFlameGrpcHost BuildGrpcHost(this FlueFlameAspNetBuilder builder, GrpcChannelOptions grpcChannelOptions)
	{
		return new FlueFlameGrpcHost(builder.TestServer, grpcChannelOptions);
	}
}