using System.Collections.Concurrent;
using FlueFlame.AspNetCore.SignalR.Helpers;

namespace FlueFlame.AspNetCore.SignalR.Services;

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