using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using SimpleGrpcService;

namespace Testing.TestData.AspNetCore.Services;

[Authorize]
public class GreeterService : Greeter.GreeterBase
{
	public GreeterService()
	{
		
	}
 
	public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
	{
		return Task.FromResult(new HelloReply
		{
			Message = "Hello " + request.Name
		});
	}
}