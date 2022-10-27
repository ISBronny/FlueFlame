# Testing SignalR Hub

## Ping-pong

We want to test the following hub:

```csharp
public class PingHub : Hub
{
	public async Task Ping(string ping)
	{
		await Clients.Caller.SendCoreAsync("Pong", new object[] {$"Pong: {ping}"});
	}
}
```

Let's use `SignalRModule`:

```csharp
[Test]
public void PingPongTest()
{
	Application.SignalR
		.CreateConnection("/hub/ping")
		.On("Pong", (string response) => response.Should().Be("Pong: test text"))
		.Send("Ping", "test text")
		.WaitForMethodCall("Pong");
}
```

Let's consider each method. With `CreateConnection` we establish a connection to the hub:

```csharp
Application.SignalR
	.CreateConnection("/hub/ping");
```

In the `On` method, we pass a function that will be called every time the server calls the `Pong` method on the client.

```csharp
Application.SignalR
	.CreateConnection("/hub/ping")
	.On("Pong", (string response) => response.Should().Be("Pong: test text"));
```

Method `Send` invokes a hub method on the server using the specified method name and arguments. Does not wait for a response from the receiver.

```csharp
Application.SignalR
	.CreateConnection("/hub/ping")
	.On("Pong", (string response) => response.Should().Be("Pong: test text"))
	.Send("Ping", "test text");
```

For the assert to work, you need to wait until the `Pong` method is called on the client:

```csharp
Application.SignalR
	.CreateConnection("/hub/ping")
	.On("Pong", (string response) => response.Should().Be("Pong: test text"))
	.Send("Ping", "test text")
	.WaitForMethodCall("Pong");
```

## Multiple Connections

FlueFlame allows you to simulate the connection of multiple clients. Consider Chat Hub:

```csharp
public class ChatHub : Hub
{
	public async Task SendMessage(string user, string message)
	{
		await Clients.All.SendAsync("OnReceiveMessage", user, message);
	}
}
```

If one client sends a message, everyone should receive it. You can test it like this:

```csharp
[Test]
public void AllClientsReceiveMessageTest()
{
	var message = new { From = "JavaBoy", Text = "Hi!"};
	
    Application.SignalR
		.CreateConnection("/hub/chat", "User_receiver1")
		.On("OnReceiveMessage", (string from, string _) => from.Should().Be(message.From))
		.On("OnReceiveMessage", (string _, string text) => text.Should().Be(message.Text))
	.Application.SignalR
		.CreateConnection("/hub/chat", "User_receiver2")
		.On("OnReceiveMessage", (string from, string _) => from.Should().Be(message.From))
		.On("OnReceiveMessage", (string _, string text) => text.Should().Be(message.Text))
	.Application.SignalR
		.CreateConnection("/hub/chat","User_Sender")
		.Send("SendMessage", message.From, message.Text)
	.Application.SignalR
		.ForCreatedConnection("User_receiver1")
		.WaitForMethodCall("OnReceiveMessage")
	.Application.SignalR
		.ForCreatedConnection("User_receiver2")
		.WaitForMethodCall("OnReceiveMessage");
}
```

With `CreateConnection` you can create a named connection:
```csharp
Application.SignalR
		.CreateConnection("/hub/chat", "User_receiver1")
```
Then you can refer to it again:
```csharp
Application.SignalR
		.ForCreatedConnection("User_receiver1")
```

We can wait until the message reaches the first recipient, and then the second:

```csharp
.Application.SignalR
		.ForCreatedConnection("User_receiver1")
		.WaitForMethodCall("OnReceiveMessage")
	.Application.SignalR
		.ForCreatedConnection("User_receiver2")
		.WaitForMethodCall("OnReceiveMessage");
```


