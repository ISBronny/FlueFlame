using System.Threading.Channels;
using FlueFlame.AspNetCore.Grpc.Modules;
using FlueFlame.Core;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.TestHost;

namespace FlueFlame.AspNetCore.Grpc;

public interface IFlueFlameGrpcHost : IFlueFlameHost
{

	/// <summary>
	/// Create gRPC connection
	/// </summary>
	/// <param name="options">Grpc Channel Options</param>
	/// <typeparam name="T">Type of gRPC Client</typeparam>
	/// <returns></returns>
	public GrpcConnectionModule<T> CreateConnection<T>(GrpcChannelOptions options = null) where T : ClientBase<T>;


	/// <summary>
	/// Sets JWT token header for authentication.
	/// </summary>
	/// <param name="token">JWT Token</param>
	/// <returns></returns>
	public FlueFlameGrpcHost UseJwtToken(string token);

}