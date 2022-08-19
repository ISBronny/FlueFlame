using System;
using FlueFlame.AspNetCore.Common;
using FlueFlame.AspNetCore.Helpers;
using FlueFlame.AspNetCore.Modules.Response;
using FlueFlame.AspNetCore.Services.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace FlueFlame.AspNetCore.Modules.SignalR;

public class SignalRModule : AspNetModuleBase
{
	private HubConnectionWrapper HubConnectionWrapper { get; set; }
	private SignalRService SignalRService { get; }
	private HubConnection HubConnection => HubConnectionWrapper.HubConnection;

	public SignalRModule(FlueFlameHost application) : base(application)
	{
		SignalRService = Application.ServiceFactory.Get<SignalRService>();
	}

	public SignalRModule ForCreatedConnection(object id)
	{
		HubConnectionWrapper = SignalRService.GetById(id);
		return this;
	}
	
	public SignalRModule CreateConnection(string hubName, object id = null)
	{
		var host = Application.TestServer.BaseAddress.Host;
		var hubConnection = new HubConnectionBuilder()
			.WithUrl($"http://{host}/{hubName.Trim('/')}", o =>
			{
				o.HttpMessageHandlerFactory = _ => Application.TestServer.CreateHandler();
			})
			.Build();
		hubConnection.StartAsync().Wait();
		HubConnectionWrapper = SignalRService.RegisterConnection(hubConnection, id);
		return this;
	}

	public SignalRModule On<T1>(string methodName, Action<T1> assert)
	{
		HubConnection.On(methodName, assert);
		HubConnection.On(methodName, () => HubConnectionWrapper.NotifyResponse(methodName));
		return this;
	}
	
	public SignalRModule On<T1, T2>(string methodName, Action<T1, T2> assert)
	{
		HubConnection.On(methodName, assert);
		HubConnection.On(methodName, () => HubConnectionWrapper.NotifyResponse(methodName));

		return this;
	}
	
	public SignalRModule On<T1, T2, T3>(string methodName, Action<T1, T2, T3> assert)
	{
		HubConnection.On(methodName, assert);
		HubConnection.On(methodName, () => HubConnectionWrapper.NotifyResponse(methodName));

		return this;
	}
	
	public SignalRModule On<T1, T2, T3, T4>(string methodName, Action<T1, T2, T3, T4> assert)
	{
		HubConnection.On(methodName, assert);
		HubConnection.On(methodName, () => HubConnectionWrapper.NotifyResponse(methodName));
		return this;
	}
	
	public SignalRModule On<T1, T2, T3, T4, T5>(string methodName, Action<T1, T2, T3, T4, T5> assert)
	{
		HubConnection.On(methodName, assert);
		HubConnection.On(methodName, () => HubConnectionWrapper.NotifyResponse(methodName));
		return this;
	}

	public SignalRModule Send(string methodName, params object[] values)
	{
		HubConnection.SendCoreAsync(methodName, values);
		return this;
	}

	public SignalRModule WaitForMethodCall(string methodName, int times = 1)
	{
		HubConnectionWrapper.WaitForMethodCall(methodName);
		return this;
	}
}