namespace FlueFlame.Core;

public abstract class FlueFlameModuleBase<THost> where THost : IFlueFlameHost
{
	public THost Host { get; }

	protected FlueFlameModuleBase(THost host)
	{
		Host = host;
	}
}

public interface IFlueFlameHost
{
	
}