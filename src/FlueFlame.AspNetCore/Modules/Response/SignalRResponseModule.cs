using FlueFlame.AspNetCore.Modules.Response.Content;
using FlueFlame.AspNetCore.Modules.Response.Content.Formatted;

namespace FlueFlame.AspNetCore.Modules.Response;

public class SignalRResponseModule : ResponseModule
{
	public SignalRResponseModule(FlueFlameHost application) : base(application)
	{
	}

	public override JsonContentResponseModule AsJson { get; }
	public override XmlContentResponseModule AsXml { get; }
	public override TextContentResponseModule AsText { get; }
}