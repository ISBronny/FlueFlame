using Grpc.Core;
using MathGrpcService;

namespace Testing.TestData.AspNetCore.Services;

public class GreatMathService : GreatMath.GreatMathBase
{
	public override Task<Number> GetSquare(Number request, ServerCallContext context)
	{
		return Task.FromResult(new Number {Value = request.Value * request.Value});
	}

	public override async Task GetPrimesLessThen(Number request, IServerStreamWriter<Number> responseStream, ServerCallContext context)
	{
		var primes = GetPrimes(request.Value);
		foreach (var prime in primes)
		{
			await responseStream.WriteAsync(new Number {Value = prime});
		}
	}

	public override async Task<Number> GetSum(IAsyncStreamReader<Number> requestStream, ServerCallContext context)
	{
		int sum = 0;
		await foreach (var number in requestStream.ReadAllAsync())
		{
			sum += number.Value;
		}

		return new Number() {Value = sum};
	}

	public override async Task GetMultiplication(IAsyncStreamReader<Number> requestStream, IServerStreamWriter<Number> responseStream, ServerCallContext context)
	{
		int prod = 1;
		await foreach (var num in requestStream.ReadAllAsync())
		{
			prod *= num.Value;
			await responseStream.WriteAsync(new Number() {Value = prod});
		}
	}
	
	static bool IsPrime(int n)
	{
		if (n <= 0)
			return false;
		
		for (int i = 2; i < n; i++)
			if (n % i == 0)
				return false;
     
		return true;
	}
	
	static IEnumerable<int> GetPrimes(int n)
	{
		for (int i = 1; i <= n; i++)
		{
			if (IsPrime(i))
				yield return i;
		}
	}
}