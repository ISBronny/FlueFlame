using FlueFlame.Core.Response.Content;
using FlueFlame.Core.Response.Content.Formatted;

namespace FlueFlame.Core.Response;

public abstract class ResponseModule<THost> : FlueFlameModuleBase<THost> where THost : IFlueFlameHost
{
	protected ResponseModule(THost host) : base(host)
	{
	}
	
	public abstract JsonContentResponseModule<THost> AsJson { get; }
	public abstract XmlContentResponseModule<THost> AsXml { get; }
	public abstract TextContentResponseModule<THost> AsText { get; }
}