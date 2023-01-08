# Getting started

FlueFlame предоставляет удобный интерфейс для тестирования всех видов запросов gRPC. Для этого он будет использовать ваш protobuf файл и генинровать сnандартный клиент [Grpc.Net.Clinet](https://www.nuget.org/packages/Grpc.Net.Client).

## Настройка проекта

Полный пример кода из этой документации можно посмотреть в GitHub [репозитории]().
У насть есть проект Examples.Api с реализацией gRPC сервера на основе ASP.NET и тестовый проект  Examples.Tests.Api.

Для начала нужно добавить в тестовый проект все зависимости для генерации клиента, как в официальной [документации](https://learn.microsoft.com/en-us/aspnet/core/tutorials/grpc/grpc-start?view=aspnetcore-7.0&tabs=visual-studio-code#add-required-nuget-packages):

```
dotnet add Examples.Tests.Api.csproj package Grpc.Net.Client
dotnet add Examples.Tests.Api.csproj package Google.Protobuf
dotnet add Examples.Tests.Api.csproj package Grpc.Tools
```

Затем добавить ссылку на protobuf файл в `Examples.Tests.Api.csproj`:

```
<ItemGroup>
    <Protobuf Include="..\Examples.Api\Protos\employees.proto" GrpcServices="Client" />
</ItemGroup>
```


`IFlueFlameGrpcHost` создаётся [по аналогии](/rest/configuration) с `IFlueFlameHttpHost`.
Добавьте в ваш TestBase класс строки:

```csharp
public abstract class TestBase : IDisposable
{
	//Previous properties

	protected IFlueFlameGrpcHost GrpcHost { get; }

    protected TestBase()
	{
        //...

        //Initialize IFlueFlameGrpcHost
        GrpcHost = builder.BuildGrpcHost();
    }

}

```

## Простой тест

В protobuf файле описан RPC получения сотрудника по его ID:

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

:::tip
gRPC не поддерживаент использование скалярных типов, таких как `string` для описания аргументов RPC. Поэтому мы будем использовать врапперы, такие как google.protobuf.StringValue.
:::

Напишем тест, который вызывает метод `GetById` и проряет, что вернулся объектв с правильным ID:

```csharp
public class EmployeeServiceTests : TestBase
{
	[Fact]
	public void GetByIdTest_Exists_ReturnsEmployee()
	{
        //Create Employee in DB
		var employee = new EmployeeTestDataBuilder(EmployeeContext)
			.Build();

		GrpcHost
			.CreateClient<EmployeeService.EmployeeServiceClient>()
			.Unary
				.Call(c => c.GetById(new StringValue { Value = employee.Guid.ToString() }))
				.Response
					.AssertThat(e => e.Guid.Should().Be(employee.Guid.ToString()));
	}
}

```

В дженерик методе CreateConnection вам нужно указать класс сгенерированного клиента. Обратите внимание, что класс клиента `EmployeeServiceClient` вложен в класс `EmployeeService`.