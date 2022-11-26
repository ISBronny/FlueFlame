# Testing gRPC

Our application has a .proto file with math functions:

```proto
syntax = "proto3";

option csharp_namespace = "MathGrpcService";

package great_math;

service GreatMath {
    
    rpc GetSquare(Number) returns (Number);
    rpc GetSum(stream Number) returns (Number);
    rpc GetPrimesLessThen(Number) returns (stream Number);
    rpc GetMultiplication(stream Number) returns (stream Number);
    
}

message Number {
    int32 value = 1;
}
```

And GreatMathService implementing these functions:

```csharp
public class GreatMathService : GreatMath.GreatMathBase
{

}
```

To create a `Number` we will use a static method:

```csharp
public static class NumberFactory
{
	public static Number FromInteger(int value)
	{
		return new Number {Value = value};
	}
}
```

`GrpcModule` is used for testing. The `CreateConnection` method establishes a connection to the server. For it, you need to specify the class of the automatically generated client.

```csharp
[Test]
public void SimpleGrpcTest()
{
	Application.gRPC
		.CreateConnection<GreatMath.GreatMathClient>()

}
```

## Unary RPC

The first GetSquare method returns the square of a number. Let's implement it:
```csharp
public class GreatMathService : GreatMath.GreatMathBase
{
	public override Task<Number> GetSquare(Number request, ServerCallContext context)
	{
		return Task.FromResult(new Number {Value = request.Value * request.Value});
	}
}
```

Check that square `5` is `25`:

```csharp
[Test]
public void SimpleGrpcTest()
{
	Application.gRPC
		.CreateConnection<GreatMath.GreatMathClient>()
		.Call(
			client => client.GetSquare(NumberFactory.FromInteger(5)),
			response => response.Value.Should().Be(25));

}

```

The `Call` method takes 2 actions: the first one will be called to send the request, and the second one will be called to check the response.

## Server streaming RPC

Let's add a `GetPrimesLessThen` method to our `GreatMathService` that returns primes less than the given one:

```csharp
public override async Task GetPrimesLessThen(Number request, IServerStreamWriter<Number> responseStream, ServerCallContext context)
{
	var primes = GetPrimes(request.Value);
	foreach (var prime in primes)
	{
		await responseStream.WriteAsync(new Number {Value = prime});
	}
}
```

Let's test it like this:

```csharp
[Test]
public void ServerSideStreamingTest()
{
	Application.gRPC
		.CreateConnection<GreatMath.GreatMathClient>()
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
```

For testing, we created a `ResponseHandler` method that receives values from `AsyncServerStreamingCall<Number>`


## Client streaming RPC

Let's add the `GetSum` method to our `GreatMathService`, which returns the sum of numbers from the stream:

```csharp
public override async Task<Number> GetSum(IAsyncStreamReader<Number> requestStream, ServerCallContext context)
{
	int sum = 0;
	await foreach (var number in requestStream.ReadAllAsync())
	{
		sum += number.Value;
	}
	return new Number() {Value = sum};
}
```

Let's test it like this:

```csharp
[Test]
public void ClientSideStreamingTest()
{
	Application.gRPC
		.CreateConnection<GreatMath.GreatMathClient>()
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
```

For testing, the `SendNumbers` method has been created, which imitates the client's value stream.


## Bidirectional streaming RPC

Let's add a `GetMultiplication` method to our `GreatMathService` that returns the product of numbers each time a new one comes in:

```csharp
public override async Task GetMultiplication(IAsyncStreamReader<Number> requestStream, IServerStreamWriter<Number> responseStream,ServerCallContext context)
{
	int prod = 1;
	await foreach (var num in requestStream.ReadAllAsync())
	{
		prod *= num.Value;
		await responseStream.WriteAsync(new Number() {Value = prod});
	}
}
```

Let's test it like this:

```csharp
[Test]
public void BidirectionalStreamingTest()
{
	Application.gRPC
		.CreateConnection<GreatMath.GreatMathClient>()
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
```

For testing, the `HandleGetMultiplication` method has been created, which implements sending, receiving values and checking them.

