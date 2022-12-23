namespace FlueFlame.AspNetCore.Grpc;

public static class FlueFlameAspNetBuilderExtensions
{
	public static IFlueFlameGrpcHost BuildGrpcHost(this FlueFlameAspNetBuilder builder)
	{
		return new FlueFlameGrpcHostBuilder(builder.TestServer)
			.Build();
	}
	
	public static IFlueFlameGrpcHost BuildGrpcHost(this FlueFlameAspNetBuilder builder, Action<FlueFlameGrpcHostBuilder> configureBuilder)
	{
		var grpcHostBuilder = new FlueFlameGrpcHostBuilder(builder.TestServer);
		configureBuilder(grpcHostBuilder);
		return grpcHostBuilder.Build();
	}
}