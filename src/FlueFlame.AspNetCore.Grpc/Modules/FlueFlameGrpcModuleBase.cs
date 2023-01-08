using FlueFlame.Core;
using Grpc.Core;

namespace FlueFlame.AspNetCore.Grpc.Modules;

public abstract class FlueFlameGrpcModuleBase<TClient> :  FlueFlameModuleBase<IFlueFlameGrpcHost> where TClient : ClientBase<TClient>
{
	protected TClient Client { get; }

	internal FlueFlameGrpcModuleBase(IFlueFlameGrpcHost host, TClient client) : base(host)
	{
		Client = client;
	}
}