using FlueFlame.AspNetCore.Grpc.Modules.Common;
using Grpc.Core;

namespace FlueFlame.AspNetCore.Grpc.Modules.ServerStreaming;

public class ServerStreamingResponseRpcModule<TClient, TResponse> : ResponseStreamRpcModule<TClient, TResponse,  ServerStreamingResponseRpcModule<TClient, TResponse>> where TClient : ClientBase<TClient> where TResponse : class
{
	internal ServerStreamingResponseRpcModule(IFlueFlameGrpcHost host, TClient client, IAsyncStreamReader<TResponse> asyncStreamReader) : base(host, client, asyncStreamReader)
	{
	}
}