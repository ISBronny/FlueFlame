using FlueFlame.AspNetCore.Grpc.Modules.BidirectionalStreaming;
using FlueFlame.AspNetCore.Grpc.Modules.ClientStreaming;
using FlueFlame.AspNetCore.Grpc.Modules.ServerStreaming;
using FlueFlame.AspNetCore.Grpc.Modules.Unary;
using Grpc.Core;

namespace FlueFlame.AspNetCore.Grpc.Modules;

public class GrpcFacadeModule<TClient> : FlueFlameGrpcModuleBase<TClient> where TClient : ClientBase<TClient>
{
	public UnaryRpcModule<TClient> Unary { get; }
	public ClientStreamingRpcModule<TClient> ClientStreaming { get; }
	public ServerStreamingRpcModule<TClient> ServerStreaming { get; }
	public BidirectionalStreamingRpcModule<TClient> Bidirectional { get; }

	public GrpcFacadeModule(IFlueFlameGrpcHost flameHost, TClient client) : base(flameHost, client)
	{
		Unary = new UnaryRpcModule<TClient>(flameHost, client);
		ClientStreaming = new ClientStreamingRpcModule<TClient>(flameHost, client);
		ServerStreaming = new ServerStreamingRpcModule<TClient>(flameHost, client);
		Bidirectional = new BidirectionalStreamingRpcModule<TClient>(flameHost, client);
	}
}