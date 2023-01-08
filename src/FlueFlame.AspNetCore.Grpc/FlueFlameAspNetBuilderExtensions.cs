namespace FlueFlame.AspNetCore.Grpc;

public static class FlueFlameAspNetBuilderExtensions
{
	public static IFlueFlameGrpcHost BuildGrpcHost(this FlueFlameAspNetBuilder builder)
	{
		return new FlueFlameGrpcHostBuilder(builder.TestServer, builder.HttpClient)
			.Build();
	}
	
	public static IFlueFlameGrpcHost BuildGrpcHost(this FlueFlameAspNetBuilder builder, Action<FlueFlameGrpcHostBuilder> configureBuilder)
	{
		var grpcHostBuilder = new FlueFlameGrpcHostBuilder(builder.TestServer, builder.HttpClient);
		configureBuilder(grpcHostBuilder);
		return grpcHostBuilder.Build();
	}
}