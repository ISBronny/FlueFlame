namespace FlueFlame.Core.Response.Content;

public class ContentResponseModule<THost> : FlueFlameModuleBase<THost> where THost : IFlueFlameHost
{
	protected string Content { get; }
	internal ContentResponseModule(THost host, string content) : base(host)
	{
		Content = content;
	}
}