using FlueFlame.AspNetCore.Grpc.Modules;
using FlueFlame.Core;
using Grpc.Core;
using Grpc.Net.Client;

namespace FlueFlame.AspNetCore.Grpc;

public interface IFlueFlameGrpcHost : IFlueFlameHost
{
	/// <summary>
	/// Create gRPC connection
	/// </summary>
	/// <typeparam name="T">Type of gRPC Client</typeparam>
	/// <returns></returns>
	public GrpcFacadeModule<T> CreateClient<T>() where T : ClientBase<T>;
	
	/// <summary>
	/// Create gRPC connection
	/// </summary>
	/// <param name="options">Grpc Channel Options</param>
	/// <typeparam name="T">Type of gRPC Client</typeparam>
	/// <returns></returns>
	public GrpcFacadeModule<T> CreateClient<T>(GrpcChannelOptions options) where T : ClientBase<T>;
	
	/// <summary>
	/// Create gRPC connection
	/// </summary>
	/// <param name="grpcChannel">Represents a gRPC channel</param>
	/// <typeparam name="T">Type of gRPC Client</typeparam>
	/// <returns></returns>
	public GrpcFacadeModule<T> CreateClient<T>(GrpcChannel grpcChannel) where T : ClientBase<T>;

}