﻿using System.Net.Mime;
using FluentAssertions;

namespace Testing.Tests.AspNet.NUnit.SignalR;

public class SignalRTests : TestBase
{
	[Test]
	public void PingPongTest()
	{
		SignalR
			.CreateConnection("/hub/ping")
			.On("Pong", (string response) => response.Should().Be("Pong: test text"))
			.Send("Ping", "test text")
			.WaitForMethodCall("Pong");
	}

	[Test]
	public void AllClientsReceiveMessageTest()
	{
		var message = new { From = "JavaBoy", Text = "Hi!"};
		
		SignalR
			.CreateConnection("/hub/chat", "User_Sender")
			.On("OnReceiveMessage", (string from, string _) => from.Should().Be(message.From))
			.On("OnReceiveMessage", (string _, string text) => text.Should().Be(message.Text))
			
			.CreateConnection("/hub/chat", "User_receiver1")
			.On("OnReceiveMessage", (string from, string _) => from.Should().Be(message.From))
			.On("OnReceiveMessage", (string _, string text) => text.Should().Be(message.Text))
			
			.CreateConnection("/hub/chat", "User_receiver2")
			.On("OnReceiveMessage", (string from, string _) => from.Should().Be(message.From))
			.On("OnReceiveMessage", (string _, string text) => text.Should().Be(message.Text))
			
			.ForCreatedConnection("User_Sender")
			.Send("SendMessage", message.From, message.Text)
			.WaitForMethodCall("OnReceiveMessage");
	}
}