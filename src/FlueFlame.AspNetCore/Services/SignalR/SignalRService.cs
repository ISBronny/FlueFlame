using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Client;

namespace FlueFlame.AspNetCore.Services.SignalR;

internal interface ISignalRService
{
	object RegisterConnection(HubConnection connection, object id = null);
	HubConnection GetHubConnectionById(object id);
	HubConnectionMethodsObserver GetHubConnectionObserverById(object id);
	bool IsConnectionExists(object id);
}

internal class SignalRService : ISignalRService
{
	private readonly Dictionary<object, (HubConnection HubConnection, HubConnectionMethodsObserver Observer)> _hubConnections = new();

	public object RegisterConnection(HubConnection connection, object id = null)
	{
		if (connection == null)
			throw new ArgumentNullException(nameof(connection));

		id ??= Guid.NewGuid();
		_hubConnections.Add(id, (connection, new HubConnectionMethodsObserver()));
		return id;
	}

	public HubConnection GetHubConnectionById(object id) => _hubConnections[id].HubConnection;
	public HubConnectionMethodsObserver GetHubConnectionObserverById(object id) => _hubConnections[id].Observer;
	public bool IsConnectionExists(object id) => _hubConnections.ContainsKey(id);
}