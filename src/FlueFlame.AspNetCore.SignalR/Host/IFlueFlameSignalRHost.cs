using FlueFlame.Core;

namespace FlueFlame.AspNetCore.SignalR.Host;

public interface IFlueFlameSignalRHost : IFlueFlameHost
{
	/// <summary>
	/// Specifies that the following methods calls are for an already existing SignalR connection.
	/// To create a connection with an ID, use the method <see cref="CreateConnection"/> with the desired ID.
	/// </summary>
	/// <param name="id">ID of created SignalR connection.</param>
	/// <returns></returns>
	public FlueFlameSignalRHost ForCreatedConnection(object id);

	/// <summary>
	/// Creates Hub Connection.
	/// <br/>
	/// FlueFlame supports creating and connecting multiple clients at the same time,
	/// so you can specify an ID to be able to access the connected connection.
	/// To work with an existing connection, use <see cref="ForCreatedConnection"/>
	/// </summary>
	/// <param name="hubRoute">Hub route</param>
	/// <param name="id">
	/// ID of connection.
	/// <br/>
	/// Note: This ID is only used inside FlueFlame and has nothing to do with SignalR's ConnectionId
	/// </param>
	/// <returns></returns>
	public FlueFlameSignalRHost CreateConnection(string hubRoute, object id = null);

	/// <summary>
	/// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
	/// Use can use assertions in this handler.
	/// </summary>
	/// <param name="methodName">The name of the hub method to define.</param>
	/// <param name="assert">Handler that will be invoked</param>
	/// <typeparam name="T1">The first argument type.</typeparam>
	/// <returns></returns>
	public FlueFlameSignalRHost On<T1>(string methodName, Action<T1> assert);

	/// <summary>
	/// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
	/// Use can use assertions in this handler.
	/// </summary>
	/// <param name="methodName">The name of the hub method to define.</param>
	/// <param name="assert">Handler that will be invoked</param>
	/// <typeparam name="T1">The first argument type.</typeparam>
	/// <typeparam name="T2">The second argument type.</typeparam>
	/// <returns></returns>
	public FlueFlameSignalRHost On<T1, T2>(string methodName, Action<T1, T2> assert);

	/// <summary>
	/// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
	/// Use can use assertions in this handler.
	/// </summary>
	/// <param name="methodName">The name of the hub method to define.</param>
	/// <param name="assert">Handler that will be invoked</param>
	/// <typeparam name="T1">The first argument type.</typeparam>
	/// <typeparam name="T2">The second argument type.</typeparam>
	/// <typeparam name="T3">The third argument type.</typeparam>
	/// <returns></returns>
	public FlueFlameSignalRHost On<T1, T2, T3>(string methodName, Action<T1, T2, T3> assert);

	/// <summary>
	/// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
	/// Use can use assertions in this handler.
	/// </summary>
	/// <param name="methodName">The name of the hub method to define.</param>
	/// <param name="assert">Handler that will be invoked</param>
	/// <typeparam name="T1">The first argument type.</typeparam>
	/// <typeparam name="T2">The second argument type.</typeparam>
	/// <typeparam name="T3">The third argument type.</typeparam>
	/// <typeparam name="T4">The fourth argument type.</typeparam>
	/// <returns></returns>
	public FlueFlameSignalRHost On<T1, T2, T3, T4>(string methodName, Action<T1, T2, T3, T4> assert);

	/// <summary>
	/// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
	/// Use can use assertions in this handler.
	/// </summary>
	/// <param name="methodName">The name of the hub method to define.</param>
	/// <param name="assert">Handler that will be invoked</param>
	/// <typeparam name="T1">The first argument type.</typeparam>
	/// <typeparam name="T2">The second argument type.</typeparam>
	/// <typeparam name="T3">The third argument type.</typeparam>
	/// <typeparam name="T4">The fourth argument type.</typeparam>
	/// <typeparam name="T5">The fifth argument type.</typeparam>
	/// <returns></returns>
	public FlueFlameSignalRHost On<T1, T2, T3, T4, T5>(string methodName, Action<T1, T2, T3, T4, T5> assert);

	/// <summary>
	/// Invokes a hub method on the server using the specified method name and arguments.
	/// </summary>
	/// <param name="methodName">The name of the server method to invoke.</param>
	/// <param name="values">The arguments used to invoke the server method.</param>
	/// <returns></returns>
	public FlueFlameSignalRHost Send(string methodName, params object[] values);

	/// <summary>
	/// Waits for the specified method to be called from the server side specified number of times (by default - 1).
	/// </summary>
	/// <param name="methodName">The name of the client method to wait.</param>
	/// <param name="times">The number of times the method should be called.</param>
	/// <returns></returns>
	public FlueFlameSignalRHost WaitForMethodCall(string methodName, int times = 1);
}