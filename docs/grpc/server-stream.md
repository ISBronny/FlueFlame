# Server streaming RPC

У нас есть protobuf файл c определённым Server streaming RPC `GetByAge`. Он возвращает поток сотрудников определённого возраста:

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

Получим сотрудников в возрасте от 35 до 38:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.ServerStreaming
	    .Call(x=>x.GetByAge(new AgeRangeRequest { From = 35, To = 38}))
```

Затем проверим, что каждый возраст каждого сотрудника в потоке соотвествует запрошенному:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.ServerStreaming
		.Call(x=>x.GetByAge(new AgeRangeRequest { From = 35, To = 38}))
		.ResponseStream
			.AssertForEach(e=>e.Age.Should().BeInRange(30, 38));
```

Метод `AssertForEach` вызовет переданную лямбду для каждого ответа в потоке.

Вы также можете отедльно проверять каждый ответ:

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

Давайте попробую вызвать этот же метод с неправильным аргуентом, где `From` больше `To`. После попытки получить первый ответ, RPC должен вернуть ошибку `InvalidArgument`. Протестировать это можно так:

```csharp

GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.ServerStreaming
		.Call(x=>x.GetByAge(new AgeRangeRequest { From = 90, To = 10}))
		.ResponseStream
			.Next()
			.AssertStatusCode(StatusCode.InvalidArgument);

```