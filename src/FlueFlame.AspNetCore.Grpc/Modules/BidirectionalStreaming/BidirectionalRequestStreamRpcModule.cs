using FlueFlame.AspNetCore.Grpc.Modules.Common;
using Grpc.Core;

namespace FlueFlame.AspNetCore.Grpc.Modules.BidirectionalStreaming;

public class BidirectionalRequestStreamRpcModule<TClient, TRequest, TResponse> :  RequestStreamRpcModule<TClient, TRequest, TResponse, BidirectionalRequestStreamRpcModule<TClient, TRequest, TResponse>> where TResponse : class where TRequest : class where TClient : ClientBase<TClient>
{
	public BidirectionalResponseStreamRpcModule<TClient, TResponse, TRequest> ResponseStream { get; internal set; }
	
	internal BidirectionalRequestStreamRpcModule(IFlueFlameGrpcHost host,
		TClient client,
		IClientStreamWriter<TRequest> clientStreamWriter) : base(host, client, clientStreamWriter)
	{
		
	}
}