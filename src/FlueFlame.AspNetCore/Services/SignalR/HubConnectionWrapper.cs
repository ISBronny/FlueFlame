using System.Collections.Concurrent;
using FlueFlame.AspNetCore.Helpers;
using Microsoft.AspNetCore.SignalR.Client;

namespace FlueFlame.AspNetCore.Services.SignalR;

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