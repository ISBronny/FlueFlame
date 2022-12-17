using FlueFlame.Http.Host;
using Newtonsoft.Json;

namespace FlueFlame.Serialization.Newtonsoft;

public static class FlueFlameHttpHostBuilderExtensions
{
	public static FlueFlameHttpHostBuilder UseNewtonsoftJsonSerializer(this FlueFlameHttpHostBuilder builder,
		JsonSerializerSettings settings = null)
	{
		builder.UseCustomJsonSerializer(new NewtonsoftJsonSerializer(settings));
		return builder;
	}
}