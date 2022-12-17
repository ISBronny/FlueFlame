namespace FlueFlame.AspNetCore.SignalR.Host;

public static class FlueFlameAspNetBuilderExtensions
{
	public static IFlueFlameSignalRHost BuildSignalRHost(this FlueFlameAspNetBuilder builder)
	{
		return new FlueFlameSignalRHost(builder.TestServer);
	}
}