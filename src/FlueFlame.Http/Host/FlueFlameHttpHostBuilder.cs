using System.Text.Json;
using FlueFlame.Core.Builders;
using FlueFlame.Core.Serialization;

namespace FlueFlame.Http.Host;

public class FlueFlameHttpHostBuilder : FlueFlameHostBuilder<FlueFlameHttpHostBuilder>
{
	private IXmlSerializer XmlSerializer { get; set; } = new XmlSerializer();
	private IJsonSerializer JsonSerializer { get; set; } = new TextJsonSerializer();
	
	public FlueFlameHttpHostBuilder UseCustomJsonSerializer(IJsonSerializer jsonSerializer)
	{
		JsonSerializer = jsonSerializer;
		return this;
	}
	
	public FlueFlameHttpHostBuilder UseCustomXmlSerializer(IXmlSerializer xmlSerializer)
	{
		XmlSerializer = xmlSerializer;
		return this;
	}

	public FlueFlameHttpHostBuilder UseTextJsonSerializer(JsonSerializerOptions options = null)
	{
		JsonSerializer = new TextJsonSerializer(options);
		return this;
	}

	public IFlueFlameHttpHost Build()
	{
		return new FlueFlameHttpHost(HttpClient, JsonSerializer, XmlSerializer);
	}
}