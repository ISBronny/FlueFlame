# Client streaming RPC

У нас есть protobuf файл c определённым Client streaming RPC `CreateEmployees`. Он создаёт множество сотрудников и затем возвращает ID всех созданных сотрудников.

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

Создаим сотрудников и вызовем нужный метод клиента:

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

Обратимся к `RequestStream` и отправим все сущности:

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
Не забудьте закрыть поток, вызывав метод `Complete()`.
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

Давайте изменим возраст второго сотрудника на отрицательный:

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

Мы можем проверить, что после отправки второго сотрудника стасус RPC изменился на InvalidArgument:

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

Также мы можем проверить не только статус код, но и сам RpcException:

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