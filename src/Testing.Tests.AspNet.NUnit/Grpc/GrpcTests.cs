using System.Net.Mime;
using FluentAssertions;
using Grpc.Core;
using MathGrpcService;

namespace Testing.Tests.AspNet.NUnit.Grpc;

public class GrpcTests : TestBase
{
	[Test]
	public void SimpleGrpcTest()
	{
		Grpc.CreateConnection<GreatMath.GreatMathClient>()
			.Call(
				client => client.GetSquare(NumberFactory.FromInteger(5)),
				response=>response.Value.Should().Be(25));
	}
	
	[Test]
	public void ClientSideStreamingTest()
	{
		Grpc.CreateConnection<GreatMath.GreatMathClient>()
			.Call(x => x.GetSum(),
				SendNumbers,
				response => response.Value.Should().Be(30));
	}

	private async Task SendNumbers(AsyncClientStreamingCall<Number, Number> streamingCall)
	{
		await streamingCall.RequestStream.WriteAsync(NumberFactory.FromInteger(2));
		await streamingCall.RequestStream.WriteAsync(NumberFactory.FromInteger(5));
		await streamingCall.RequestStream.WriteAsync(NumberFactory.FromInteger(10));
		await streamingCall.RequestStream.WriteAsync(NumberFactory.FromInteger(13));
		await streamingCall.RequestStream.CompleteAsync();
	}

	[Test]
	public void ServerSideStreamingTest()
	{
		Grpc.CreateConnection<GreatMath.GreatMathClient>()
			.Call(
				client => client.GetPrimesLessThen(NumberFactory.FromInteger(25)),
				ResponseHandler);
	}

	private async Task ResponseHandler(AsyncServerStreamingCall<Number> streamingCall)
	{
		var values = await streamingCall.ResponseStream.ReadAllAsync().ToListAsync();
		values.Select(x => x.Value)
			.Should()
			.BeEquivalentTo(new List<int>
			{
				1, 2, 3, 5, 7, 11, 13, 17, 19, 23
			});
	}

	[Test]
	public void BidirectionalStreamingTest()
	{
		Grpc.CreateConnection<GreatMath.GreatMathClient>()
			.Call(x => x.GetMultiplication(), HandleGetMultiplication);
	}

	private static async Task HandleGetMultiplication(AsyncDuplexStreamingCall<Number, Number> call)
	{
		await call.RequestStream.WriteAsync(NumberFactory.FromInteger(10));
		await call.RequestStream.WriteAsync(NumberFactory.FromInteger(5));
		await call.ResponseStream.MoveNext();
		call.ResponseStream.Current.Value.Should().Be(10);
		await call.ResponseStream.MoveNext();
		call.ResponseStream.Current.Value.Should().Be(50);
		await call.RequestStream.WriteAsync(NumberFactory.FromInteger(3));
		await call.ResponseStream.MoveNext();
		call.ResponseStream.Current.Value.Should().Be(150);
	}
}

public static class NumberFactory
{
	public static Number FromInteger(int value)
	{
		return new Number {Value = value};
	}
}