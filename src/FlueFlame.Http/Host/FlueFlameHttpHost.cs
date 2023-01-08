using FlueFlame.Core.Serialization;
using FlueFlame.Http.Modules;

namespace FlueFlame.Http.Host;

public class FlueFlameHttpHost : IFlueFlameHttpHost
{
	public HttpClient HttpClient { get; set; }
	IJsonSerializer IFlueFlameHttpHost.JsonSerializer { get; set; }
	IXmlSerializer IFlueFlameHttpHost.XmlSerializer { get; set; }

	public FlueFlameHttpHost(HttpClient httpClient, IJsonSerializer jsonSerializer, IXmlSerializer xmlSerializer)
	{
		HttpClient = httpClient;
		((IFlueFlameHttpHost)this).JsonSerializer = jsonSerializer;
		((IFlueFlameHttpHost)this).XmlSerializer = xmlSerializer;
	}

	public HttpModule Delete => new(this, HttpMethod.Delete);
	public HttpModule Get => new(this, HttpMethod.Get);
	public HttpModule Head => new(this, HttpMethod.Head);
	public HttpModule Options => new(this, HttpMethod.Options);
	public HttpModule Patch => new(this, HttpMethod.Patch);
	public HttpModule Post => new(this, HttpMethod.Post);
	public HttpModule Put => new(this, HttpMethod.Put);
	public HttpModule Trace => new(this, HttpMethod.Trace);
}