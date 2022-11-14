using System;
using System.Collections.Concurrent;
using FlueFlame.AspNetCore.Helpers;

namespace FlueFlame.AspNetCore.Services.SignalR;

internal class HubConnectionMethodsObserver
{
	private readonly ConcurrentDictionary<string, bool> _invoked = new();
	
	public void NotifyResponse(string methodName)
	{
		if (!_invoked.ContainsKey(methodName))
			_invoked.TryAdd(methodName, true);
		else
			_invoked[methodName] = true;
	}

	public void WaitForMethodCall(string methodName, TimeSpan? timeout = null)
	{
		WaitHelper.WaitUntil(() => _invoked.ContainsKey(methodName) && _invoked[methodName], timeout);
		_invoked[methodName] = false;
	}
}