using FlueFlame.Core;
using FlueFlame.Core.Serialization;
using FlueFlame.Http.Modules;

namespace FlueFlame.Http.Host;

public interface IFlueFlameHttpHost : IFlueFlameHost
{
	internal IJsonSerializer JsonSerializer { get; set; }
	internal IXmlSerializer XmlSerializer { get; set; }
	public HttpClient HttpClient { get; }
	
	public HttpModule Delete { get; }
	public HttpModule Get { get; }
	public HttpModule Head { get; }
	public HttpModule Options { get; }
	public HttpModule Patch { get; }
	public HttpModule Post { get; }
	public HttpModule Put { get; }
	public HttpModule Trace { get; }
}