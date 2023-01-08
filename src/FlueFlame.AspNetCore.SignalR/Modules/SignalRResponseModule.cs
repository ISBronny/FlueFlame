using FlueFlame.AspNetCore.SignalR.Host;
using FlueFlame.Core.Response;
using FlueFlame.Core.Response.Content;
using FlueFlame.Core.Response.Content.Formatted;

namespace FlueFlame.AspNetCore.SignalR.Modules;

public class SignalRResponseModule : ResponseModule<IFlueFlameSignalRHost>
{
	public SignalRResponseModule(IFlueFlameSignalRHost application) : base(application)
	{
	}

	public override JsonContentResponseModule<IFlueFlameSignalRHost> AsJson { get; }
	public override XmlContentResponseModule<IFlueFlameSignalRHost> AsXml { get; }
	public override TextContentResponseModule<IFlueFlameSignalRHost> AsText { get; }
}