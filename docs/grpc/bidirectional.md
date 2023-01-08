# Bidirectional streaming RPC

We have a protobuf file with Server streaming RPC `GetByIds` defined. It takes a stream ID and returns a stream of the corresponding employees:

```
syntax = "proto3";

import "google/protobuf/wrappers.proto";

package Examples.Grpc;

service EmployeeService {
  rpc GetByIds(stream google.protobuf.StringValue) returns (stream Employee);
}

message Employee {
  string guid = 1;
  string full_name = 2;
  string position = 3;
  int32 age = 4;
}
```

## Call Bidirectional streaming RPC

Let's call the required method:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.Bidirectional
	    .Call(x=>x.GetByIds());
```

In the following example, we send two requests with an existing employee ID, get it, send a random ID, and check that the RPC returned a `NotFound` error:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.Bidirectional
		.Call(x=>x.GetByIds())
		.RequestStream
			.Write(new StringValue() {Value = "c8a24c0d-7783-4e24-ae06-8742e4a9a039"})
		.ResponseStream
			.Next()
			.AssertCurrent(resp=>resp.FullName.Should().Be(employee.FullName))
		.RequestStream
			.Write(Guid.NewGuid())
		.ResponseStream
			.Next()
			.AssertStatusCode(StatusCode.NotFound);
```