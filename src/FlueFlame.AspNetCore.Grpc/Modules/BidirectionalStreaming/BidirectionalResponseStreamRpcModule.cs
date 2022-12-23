using FlueFlame.AspNetCore.Grpc.Modules.Common;
using Grpc.Core;

namespace FlueFlame.AspNetCore.Grpc.Modules.BidirectionalStreaming;

public class BidirectionalResponseStreamRpcModule<TClient, TResponse, TRequest>
	: ResponseStreamRpcModule<TClient, TResponse, TRequest, BidirectionalResponseStreamRpcModule<TClient, TResponse, TRequest>>
	where TClient : ClientBase<TClient>
	where TResponse : class
	where TRequest : class
{
	public BidirectionalRequestStreamRpcModule<TClient, TRequest, TResponse> RequestStream { get; internal set; }

	internal BidirectionalResponseStreamRpcModule(IFlueFlameGrpcHost host,
		TClient client,
		IAsyncStreamReader<TResponse> asyncStreamReader
	) : base(host, client, asyncStreamReader)
	{
		
	}
}