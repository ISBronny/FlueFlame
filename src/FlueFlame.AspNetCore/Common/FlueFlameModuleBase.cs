namespace FlueFlame.AspNetCore.Common;

public abstract class FlueFlameModuleBase
{
	public FlueFlameHost Application { get; }

	protected FlueFlameModuleBase(FlueFlameHost application)
	{
		Application = application;
	}
}