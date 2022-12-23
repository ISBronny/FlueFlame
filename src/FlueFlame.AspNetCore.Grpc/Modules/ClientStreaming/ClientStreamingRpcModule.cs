using FlueFlame.AspNetCore.Grpc.Modules.Common;
using Grpc.Core;

namespace FlueFlame.AspNetCore.Grpc.Modules.ClientStreaming;

public class ClientStreamingRpcModule<TClient> : FlueFlameGrpcModuleBase<TClient> where TClient : ClientBase<TClient>
{
	internal ClientStreamingRpcModule(IFlueFlameGrpcHost host, TClient client) : base(host, client)
	{
	}
	
	internal ClientStreamingRpcModule(ClientStreamingRpcModule<TClient> module) : this(module.Host, module.Client)
	{
	}
	
	public ClientStreamingRpcModule<TClient, TRequest, TResponse> Call<TRequest, TResponse>(Func<TClient, AsyncClientStreamingCall<TRequest, TResponse>> action) where TRequest : class where TResponse : class
	{
		var streamingCall = action(Client);
		return new ClientStreamingRpcModule<TClient, TRequest, TResponse>(this, streamingCall);
	}
}

public class ClientStreamingRpcModule<TClient, TRequest, TResponse> : ClientStreamingRpcModule<TClient> where TClient : ClientBase<TClient> where TRequest : class where TResponse : class
{
	private readonly AsyncClientStreamingCall<TRequest, TResponse> _call;
	public ClientStreamingRequestRpcModule<TClient, TRequest, TResponse> RequestStream { get; }

	public RpcResponseModule<TClient, TResponse> Response
	{
		get
		{
			try
			{
				_call.RequestStream.CompleteAsync().ConfigureAwait(false).GetAwaiter().GetResult();
				return new RpcResponseModule<TClient, TResponse>(Host, Client, _call.ResponseAsync.Result);
			}
			catch (AggregateException exception) when (exception.InnerException?.GetType() == typeof(RpcException))
			{
				return new RpcResponseModule<TClient, TResponse>(Host, Client, exception.InnerException as RpcException);
			}
		}
	}
	
	internal ClientStreamingRpcModule(ClientStreamingRpcModule<TClient> module, AsyncClientStreamingCall<TRequest, TResponse> streamingCall) : base(module)
	{
		_call = streamingCall;
		RequestStream = new ClientStreamingRequestRpcModule<TClient, TRequest, TResponse>(Host, Client, streamingCall);
	}
}