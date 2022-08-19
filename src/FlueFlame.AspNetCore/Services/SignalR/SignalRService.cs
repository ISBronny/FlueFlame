using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FlueFlame.AspNetCore.Helpers;
using Microsoft.AspNetCore.SignalR.Client;

namespace FlueFlame.AspNetCore.Services.SignalR;

internal class SignalRService
{
	private readonly Dictionary<object, HubConnectionWrapper> _hubConnections = new();

	public HubConnectionWrapper RegisterConnection(HubConnection connection, object id = null)
	{
		var hubConnection = new HubConnectionWrapper(connection);
		_hubConnections.Add(id ?? Guid.NewGuid(), hubConnection);
		return hubConnection;
	}

	public HubConnectionWrapper GetById(object id) => _hubConnections[id];
}

internal class HubConnectionWrapper
{
	public readonly HubConnection HubConnection;
	private readonly ConcurrentDictionary<string, bool> _invoked = new();

	public HubConnectionWrapper(HubConnection hubConnection)
	{
		HubConnection = hubConnection;
	}

	public void NotifyResponse(string methodName)
	{
		if (_invoked.ContainsKey(methodName))
			_invoked.TryAdd(methodName, true);
		else
			_invoked[methodName] = true;
	}

	public void WaitForMethodCall(string methodName)
	{
		WaitHelper.WaitUntil(() => _invoked.ContainsKey(methodName) && _invoked[methodName]);
		_invoked[methodName] = false;
	}
}