namespace FlueFlame.AspNetCore.Common;

public abstract class FlueFlameModuleBase
{
	public IFlueFlameHost Application { get; }

	protected FlueFlameModuleBase(IFlueFlameHost application)
	{
		Application = application;
	}
}