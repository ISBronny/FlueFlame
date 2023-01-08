# Unary RPC

У нас есть protobuf файл c определённым Unary RPC `GetById`:

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

FluFlame сам создаст клиент с нужными `GrpcChannel` и `GrpcChannelOptions`, однако мы можете указать кастомные:

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

После того как вы создали клиент вам нужно выбрать один из четрырёх видов RPC. Доступ к разным RPC предоставляется через 4 свойства: `Unary`, `ClientStreaming`, `ServerStreaming` и `Bidirectional`. 
После выбора вида RPC вызовете метод `Call` с нужным методом клиента:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.Unary
		.Call(c => c.GetById(new StringValue() {Value = "c8a24c0d-7783-4e24-ae06-8742e4a9a039"}));
```

## Assert Response

Аналогично тестированию REST, обратимся к свойству Response, чтобы получить доступ к ответу:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.Unary
		.Call(c => c.GetById(new StringValue() {Value = "c8a24c0d-7783-4e24-ae06-8742e4a9a039"}));
        .Response;
```

Проверим статус код запроса и ответ:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.Unary
		.Call(c => c.GetById(new StringValue() {Value = "c8a24c0d-7783-4e24-ae06-8742e4a9a039"}));
        .Response
            .AssertStatusCode(StatusCode.OK)
			.AssertThat(e => e.Age.Should().Be(21));
```