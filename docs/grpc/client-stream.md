# Client streaming RPC

We have a protobuf file with a Client streaming RPC `CreateEmployees` defined. It creates a set of employees and then returns the ID of all created employees.

```
syntax = "proto3";

import "google/protobuf/wrappers.proto";

package Examples.Grpc;

service EmployeeService {
  rpc CreateEmployees(stream Employee) returns (CreatedEmployeesIdsResponse);
}

message Employee {
  string guid = 1;
  string full_name = 2;
  string position = 3;
  int32 age = 4;
}

message CreatedEmployeesIdsResponse {
  repeated string guid = 1;
}
```

## Call Client streaming RPC

Create employees and call the desired client method:

```csharp
var employees = new[]
{
	new Employee()
	{
		Guid = Guid.NewGuid().ToString(),
		Age = 23,
		FullName = "Elon Musk"
	},
	new Employee()
	{
		Guid = Guid.NewGuid().ToString(),
		Age = 54,
		FullName = "Neil Armstrong"
	}
};

GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.ClientStreaming
		.Call(x=>x.CreateEmployees());
```

Let's turn to `RequestStream` and send all the entities:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.ClientStreaming
		.Call(x=>x.CreateEmployees())
		.RequestStream
            //All at once
			.WriteMany(employees)
            //Or each separately
            //.Write(employees[0])
            //.Write(employees[1])
            .Complete()
```

:::warning
Don't forget to close the stream by calling the `Complete()` method.
:::

Проверим, что нам пришли верные Guid:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.ClientStreaming
		.Call(x=>x.CreateEmployees())
		.RequestStream
			.WriteMany(employees)
            .Complete()
        .Response
			.AssertThat(resp => resp.Guid.Should().BeEquivalentTo(employees.Select(e=>e.Guid)));

```


## Handling Errors

Let's change the age of the second employee to negative:

```csharp

var employees = new[]
{
	new Employee()
	{
		Guid = Guid.NewGuid().ToString(),
		Age = 23,
		FullName = "Elon Musk"
	},
	new Employee()
	{
		Guid = Guid.NewGuid().ToString(),
		Age = -12,
		FullName = "Neil Armstrong"
	}
};

```

We can check that after sending the second employee, the RPC status changed to `InvalidArgument`:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.ClientStreaming
		.Call(x=>x.CreateEmployees())
		.RequestStream
			.WriteMany(employees)
        .Response
			.AssertStatusCode(StatusCode.InvalidArgument)
```

We can also check not only the status code, but also the RpcException:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.ClientStreaming
		.Call(x=>x.CreateEmployees())
		.RequestStream
			.WriteMany(employees)
		.Response
			.AssertError(exception => exception.Status.Detail.Should().Be("Age can't be negative."));
```