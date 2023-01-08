using Microsoft.AspNetCore.SignalR.Client;

namespace FlueFlame.AspNetCore.SignalR.Services;

internal interface ISignalRService
{
	object RegisterConnection(HubConnection connection, object id = null);
	HubConnection GetHubConnectionById(object id);
	HubConnectionMethodsObserver GetHubConnectionObserverById(object id);
	bool IsConnectionExists(object id);
}