using FluentAssertions;
using SimpleGrpcService;
using Testing.Tests.AspNet.NUnit.Authentication.JWT;

namespace Testing.Tests.AspNet.NUnit.Grpc;

public class AuthTests : TestBase
{
	// [Test]
	// public void AuthWithJwtTest()
	// {
	// 	Http
	// 		.CreateJwt("admin@gmail.com", "12345", out var token);
	// 	Grpc
	// 		.UseJwtToken(token)
	// 		.CreateConnection<Greeter.GreeterClient>()
	// 		.Unary(
	// 			client => client.SayHello(new HelloRequest { Name = "Jane"}),
	// 			response=>response.Message.Should().Be("Hello Jane"));
	// }
}