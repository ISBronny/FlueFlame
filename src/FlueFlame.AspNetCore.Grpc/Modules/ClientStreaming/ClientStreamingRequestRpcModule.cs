using FlueFlame.AspNetCore.Grpc.Modules.Common;
using Grpc.Core;

namespace FlueFlame.AspNetCore.Grpc.Modules.ClientStreaming;

public class ClientStreamingRequestRpcModule<TClient, TRequest, TResponse> : RequestStreamRpcModule<TClient, TRequest, TResponse,  ClientStreamingRequestRpcModule<TClient, TRequest, TResponse>> where TResponse : class where TRequest : class where TClient : ClientBase<TClient>
{
	private readonly AsyncClientStreamingCall<TRequest, TResponse> _call;

	public RpcResponseModule<TClient, TResponse> Response
	{
		get
		{
			_call.RequestStream.CompleteAsync().Wait();
			try
			{
				return new RpcResponseModule<TClient, TResponse>(Host, Client, _call.ResponseAsync.Result);
			}
			catch (AggregateException exception) when (exception.InnerException?.GetType() == typeof(RpcException))
			{
				return new RpcResponseModule<TClient, TResponse>(Host, Client, exception.InnerException as RpcException);
			}
		}
	}
	
	internal ClientStreamingRequestRpcModule(IFlueFlameGrpcHost host, TClient client, AsyncClientStreamingCall<TRequest, TResponse> call) : base(host, client, call.RequestStream)
	{
		_call = call;
	}
}