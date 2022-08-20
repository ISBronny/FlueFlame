using FlueFlame.AspNetCore.Common;

namespace FlueFlame.AspNetCore.Modules.Response.Content;

public class ContentResponseModule : AspNetModuleBase
{
	protected string Content { get; }
	internal ContentResponseModule(FlueFlameHost application, string content) : base(application)
	{
		Content = content;
	}
}