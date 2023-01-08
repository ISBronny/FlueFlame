# Getting started

FlueFlame provides a convenient interface for testing all kinds of gRPC requests. To do this, it will use your protobuf file and generate the standard client [Grpc.Net.Clinet](https://www.nuget.org/packages/Grpc.Net.Client).

## Project setup

You can view the complete code sample from this documentation in the GitHub [repositories]().
We have an `Examples.Api` project with an ASP.NET-based gRPC server implementation and an `Examples.Tests.Api` test project.

First you need to add to the test project all the dependencies for generating the client, as in the official [documentation](https://learn.microsoft.com/en-us/aspnet/core/tutorials/grpc/grpc-start?view=aspnetcore-7.0&tabs=visual-studio-code#add-required-nuget-packages):

```
dotnet add Examples.Tests.Api.csproj package Google.Protobuf
dotnet add Examples.Tests.Api.csproj package Grpc.Tools
```

:::tip
**Grpc.Net.Client** comes with **FlueFlame.AspNetCore.Grpc** so you don't need to add it as a dependency
:::

Then add a link to the protobuf file in `Examples.Tests.Api.csproj`:

```
<ItemGroup>
    <Protobuf Include="..\Examples.Api\Protos\employees.proto" GrpcServices="Client" />
</ItemGroup>
```


`IFlueFlameGrpcHost` is created [similarly](/rest/configuration) to `IFlueFlameHttpHost`.
Add to your TestBase the line class:

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

## Simple test

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
gRPC does not support the use of scalar types such as `string` to describe RPC arguments. Therefore, we will use wrappers such as google.protobuf.StringValue.
:::

Let's write a test that calls the `GetById` method and checks that an object with the correct ID has returned:

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

In the Generic CreateConnection method, you need to specify the class of the generated client. Note that the `EmployeeServiceClient` client class is nested within the `EmployeeService` class.