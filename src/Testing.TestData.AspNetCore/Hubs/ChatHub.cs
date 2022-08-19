using Microsoft.AspNetCore.SignalR;

namespace Testing.TestData.AspNetCore.Hubs;

public class ChatHub : Hub
{
	public async Task SendMessage(string user, string message)
	{
		await Clients.All.SendAsync("OnReceiveMessage", user, message);
	}

	public Task SendMessageToCaller(string message)
	{
		return Clients.Caller.SendAsync("OnReceiveMessage", message);
	}

	public Task SendMessageToGroups(string message)
	{
		List<string> groups = new List<string>() { "SignalR Users" };
		return Clients.Groups(groups).SendAsync("OnReceiveMessage", message);
	}

	public override async Task OnConnectedAsync()
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception exception)
	{
		await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
		await base.OnDisconnectedAsync(exception);
	}
}