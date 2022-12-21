using FlueFlame.AspNetCore.Grpc.Modules.Common;
using Grpc.Core;

namespace FlueFlame.AspNetCore.Grpc.Modules.Unary;

public class UnaryRpcModule<TClient> : FlueFlameGrpcModuleBase<TClient> where TClient : ClientBase<TClient>
{
	internal UnaryRpcModule(IFlueFlameGrpcHost host, TClient client) : base(host, client)
	{
		
	}
	internal UnaryRpcModule(UnaryRpcModule<TClient> module) : base(module.Host, module.Client)
	{
		
	}

	public UnaryRpcModule<TClient, TResponse> Call<TResponse>(Func<TClient, TResponse> action)
	{
		try
		{
			return new UnaryRpcModule<TClient, TResponse>(this,  action(Client));
		}
		catch (RpcException e)
		{
			return new UnaryRpcModule<TClient, TResponse>(this, e);
		}
	}

}

public class UnaryRpcModule<TClient, TResponse> : UnaryRpcModule<TClient> where TClient : ClientBase<TClient>
{
	public RpcResponseModule<TClient, TResponse> Response { get; }

	internal UnaryRpcModule(UnaryRpcModule<TClient> module, TResponse response) : base(module)
	{
		Response = new RpcResponseModule<TClient, TResponse>(Host, Client, response);
	}
	
	internal UnaryRpcModule(UnaryRpcModule<TClient> module, RpcException exception) : base(module)
	{
		Response = new RpcResponseModule<TClient, TResponse>(Host, Client, exception);
	}
}