using System;
using FlueFlame.AspNetCore.Common;
using FlueFlame.AspNetCore.Services.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace FlueFlame.AspNetCore.Modules.SignalR;

public sealed class SignalRModule : FlueFlameModuleBase
{
	private HubConnectionWrapper HubConnectionWrapper { get; set; }
	private SignalRService SignalRService { get; }
	private HubConnection HubConnection => HubConnectionWrapper.HubConnection;

	internal SignalRModule(IFlueFlameHost application) : base(application)
	{
		SignalRService = Application.ServiceFactory.Get<SignalRService>();
	}

	/// <summary>
	/// Specifies that the following methods calls are for an already existing SignalR connection.
	/// To create a connection with an ID, use the method <see cref="CreateConnection"/> with the desired ID.
	/// </summary>
	/// <param name="id">ID of created SignalR connection.</param>
	/// <returns></returns>
	public SignalRModule ForCreatedConnection(object id)
	{
		HubConnectionWrapper = SignalRService.GetById(id);
		return this;
	}
	
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
	public SignalRModule CreateConnection(string hubRoute, object id = null)
	{
		var host = Application.TestServer.BaseAddress.Host;
		var hubConnection = new HubConnectionBuilder()
			.WithUrl($"http://{host}/{hubRoute.Trim('/')}", o =>
			{
				o.HttpMessageHandlerFactory = _ => Application.TestServer.CreateHandler();
			})
			.Build();
		hubConnection.StartAsync().Wait();
		HubConnectionWrapper = SignalRService.RegisterConnection(hubConnection, id);
		return this;
	}

	/// <summary>
	/// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
	/// Use can use assertions in this handler.
	/// </summary>
	/// <param name="methodName">The name of the hub method to define.</param>
	/// <param name="assert">Handler that will be invoked</param>
	/// <typeparam name="T1">The first argument type.</typeparam>
	/// <returns></returns>
	public SignalRModule On<T1>(string methodName, Action<T1> assert)
	{
		HubConnection.On(methodName, assert);
		HubConnection.On(methodName, () => HubConnectionWrapper.NotifyResponse(methodName));
		return this;
	}
	
	/// <summary>
	/// Registers a handler that will be invoked when the hub method with the specified method name is invoked.
	/// Use can use assertions in this handler.
	/// </summary>
	/// <param name="methodName">The name of the hub method to define.</param>
	/// <param name="assert">Handler that will be invoked</param>
	/// <typeparam name="T1">The first argument type.</typeparam>
	/// <typeparam name="T2">The second argument type.</typeparam>
	/// <returns></returns>
	public SignalRModule On<T1, T2>(string methodName, Action<T1, T2> assert)
	{
		HubConnection.On(methodName, assert);
		HubConnection.On(methodName, () => HubConnectionWrapper.NotifyResponse(methodName));

		return this;
	}
	
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
	public SignalRModule On<T1, T2, T3>(string methodName, Action<T1, T2, T3> assert)
	{
		HubConnection.On(methodName, assert);
		HubConnection.On(methodName, () => HubConnectionWrapper.NotifyResponse(methodName));

		return this;
	}
	
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
	public SignalRModule On<T1, T2, T3, T4>(string methodName, Action<T1, T2, T3, T4> assert)
	{
		HubConnection.On(methodName, assert);
		HubConnection.On(methodName, () => HubConnectionWrapper.NotifyResponse(methodName));
		return this;
	}
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
	public SignalRModule On<T1, T2, T3, T4, T5>(string methodName, Action<T1, T2, T3, T4, T5> assert)
	{
		HubConnection.On(methodName, assert);
		HubConnection.On(methodName, () => HubConnectionWrapper.NotifyResponse(methodName));
		return this;
	}

	/// <summary>
	/// Invokes a hub method on the server using the specified method name and arguments.
	/// </summary>
	/// <param name="methodName">The name of the server method to invoke.</param>
	/// <param name="values">The arguments used to invoke the server method.</param>
	/// <returns></returns>
	public SignalRModule Send(string methodName, params object[] values)
	{
		HubConnection.SendCoreAsync(methodName, values);
		return this;
	}

	/// <summary>
	/// Waits for the specified method to be called from the server side specified number of times (by default - 1).
	/// </summary>
	/// <param name="methodName">The name of the client method to wait.</param>
	/// <param name="times">The number of times the method should be called.</param>
	/// <returns></returns>
	public SignalRModule WaitForMethodCall(string methodName, int times = 1)
	{
		for (var i = 0; i < times; i++) 
			HubConnectionWrapper.WaitForMethodCall(methodName);

		return this;
	}
}