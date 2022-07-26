﻿using FluentAssertions;
using SimpleGrpcService;
using Testing.Tests.AspNet.NUnit.Authentication.JWT;

namespace Testing.Tests.AspNet.NUnit.Grpc;

public class AuthTests : TestBase
{
	[Test]
	public void AuthWithJwtTest()
	{
		Application
			.CreateJwt("admin@gmail.com", "12345", out var token)
		.gRPC
			.UseJwtToken(token)
			.CreateConnection<Greeter.GreeterClient>()
			.Call(
				client => client.SayHello(new HelloRequest { Name = "Jane"}),
				response=>response.Message.Should().Be("Hello Jane"));
	}
}