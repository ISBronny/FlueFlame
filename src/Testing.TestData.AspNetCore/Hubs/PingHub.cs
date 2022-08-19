using Microsoft.AspNetCore.SignalR;

namespace Testing.TestData.AspNetCore.Hubs;

public class PingHub : Hub
{
	public async Task Ping(string ping)
	{
		await Clients.Caller.SendCoreAsync("Pong", new object[] {$"Pong: {ping}"});
	}
}