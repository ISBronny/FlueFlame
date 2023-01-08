# Unary RPC

We have a protobuf file with a Unary RPC `GetById` defined:

```
syntax = "proto3";

import "google/protobuf/wrappers.proto";

package Examples.Grpc;

service EmployeeService {
  rpc GetById(google.protobuf.StringValue) returns (Employee);
}

message Employee {
  string guid = 1;
  string full_name = 2;
  string position = 3;
  int32 age = 4;
}
```

## Create Client

Для начала нужно создать клиент:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
```

FluFlame itself will create a client with the necessary `GrpcChannel` and `GrpcChannelOptions`, however, we can specify custom ones:

```csharp

GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>(new GrpcChannelOptions()
	{
		Credentials = ChannelCredentials.Insecure
	});

// Or

GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>(
		GrpcChannel.ForAddress("https://myaddress.com",
			new GrpcChannelOptions()));
```

## Call RPC

After you have created a client, you need to choose one of the four types of RPC. Access to different RPCs is provided through 4 properties: `Unary`, `ClientStreaming`, `ServerStreaming` and `Bidirectional`.
After selecting the RPC type, call the `Call` method with the desired client method:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.Unary
		.Call(c => c.GetById(new StringValue() {Value = "c8a24c0d-7783-4e24-ae06-8742e4a9a039"}));
```

## Assert Response

Similar to REST testing, let's access the Response property to access the response:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.Unary
		.Call(c => c.GetById(new StringValue() {Value = "c8a24c0d-7783-4e24-ae06-8742e4a9a039"}));
        .Response;
```

Check the status of the request code and response:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.Unary
		.Call(c => c.GetById(new StringValue() {Value = "c8a24c0d-7783-4e24-ae06-8742e4a9a039"}));
        .Response
            .AssertStatusCode(StatusCode.OK)
			.AssertThat(e => e.Age.Should().Be(21));
```