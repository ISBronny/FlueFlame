# Server streaming RPC

We have a protobuf file with Server streaming RPC `GetByAge` defined. It returns a stream of employees of a certain age:

```
syntax = "proto3";

package Examples.Grpc;

service EmployeeService {
  rpc GetByAge(AgeRangeRequest) returns (stream Employee);
}

message Employee {
  string guid = 1;
  string full_name = 2;
  string position = 3;
  int32 age = 4;
}

message AgeRangeRequest {
  int32 from = 1;
  int32 to = 2;
}
```

## Call Server streaming RPC

Let's get employees aged 35 to 38:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.ServerStreaming
	    .Call(x=>x.GetByAge(new AgeRangeRequest { From = 35, To = 38}))
```

Then we check that each age of each employee in the stream matches the requested one:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.ServerStreaming
		.Call(x=>x.GetByAge(new AgeRangeRequest { From = 35, To = 38}))
		.ResponseStream
			.AssertForEach(e=>e.Age.Should().BeInRange(30, 38));
```

The `AssertForEach` method will call the passed lambda for every response in the stream.

You can also check each response individually:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.ServerStreaming
		.Call(x=>x.GetByAge(new AgeRangeRequest { From = 35, To = 38}))
		.ResponseStream
			.Next()
			.AssertCurrent(employee => employee.Age.Should().Be(35))
			.Next()
			.AssertCurrent(employee => employee.Age.Should().Be(37))
			.AssertEndOfStream();
```

## Handling Errors

Let's try calling the same method with the wrong argument, where `From` is greater than `To`. After trying to get the first response, the RPC should return an `InvalidArgument` error. You can test it like this:
```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.ServerStreaming
		.Call(x=>x.GetByAge(new AgeRangeRequest { From = 90, To = 10}))
		.ResponseStream
			.Next()
			.AssertStatusCode(StatusCode.InvalidArgument);

```