using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Client;

namespace FlueFlame.AspNetCore.Services.SignalR;

internal interface ISignalRService
{
	HubConnectionWrapper RegisterConnection(HubConnection connection, object id = null);
	HubConnectionWrapper GetById(object id);
}

internal class SignalRService : ISignalRService
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