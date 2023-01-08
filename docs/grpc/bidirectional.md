# Bidirectional streaming RPC

У нас есть protobuf файл c определённым Server streaming RPC `GetByIds`. Он принимает поток ID и возвращает поток соотвествующих сотрудников:

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

Вызовем нужный метод:

```csharp
GrpcHost
	.CreateClient<EmployeeService.EmployeeServiceClient>()
	.Bidirectional
	    .Call(x=>x.GetByIds());
```

В слеудющем примере мы отправляем два запроса с сущетсвующим ID сотрудника, получаем его, отправляем случайный ID и проверяем что RPC вернул ошибку `NotFound`:

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