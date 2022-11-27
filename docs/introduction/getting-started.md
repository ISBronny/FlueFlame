# Getting Started

This section will help you build a simple test for already existing ASP.NET application.

::: warning 
FlueFlame only supports .NET 6.0 or greater.
:::


## Step. 1: Create Unit test project

You can use any test framework like xUnit, NUnit or MSTest. All examples in the documentation will be written in NUnit.

Add to your test project a NuGet Package [FlueFlame.AspNet](https://www.nuget.org/packages/FlueFlame.AspNet/):

```
dotnet add package FlueFlame.AspNet --version 0.2.0
```

## Step. 2: Create TestBase class

All tests will inherit from the `TestBase` base class:

```csharp
public abstract class TestBase
{
    protected readonly IFlueFlameHost Application;

    public TestBase()
    {
        var webApp = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                //Configure your services here
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                             typeof(DbContextOptions<EmployeeContext>));

                    services.Remove(descriptor);

                    services.AddDbContext<EmployeeContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });
                });


                // override the environment if you need to
                builder.UseEnvironment("Testing");
            });

        Application = TestApplicationBuilder.CreateDefaultBuilder(webApp)
            .UseNewtonsoftJson()
            .Build();
    }
}
```
In the constructor, you need to create a `WebApplicationFactory`, specifying your Program class.

::: tip

Since .NET 6, the `Program` class has been deprecated. It can be declared by adding the following code in the end of `Program.cs` file:
```csharp
namespace Testing.TestData.AspNetCore
{
    public partial class Program {}
}
```
:::

Use the `WithWebHostBuilder` method to configure your application. For example, use `InMemoryDatabase` if necessary. Read more in the official [documentation](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0#customize-webapplicationfactory).

`TestApplicationBuilder` creates and configures `IFlueFlameHost`. For example, you can call the `UseNewtonsoftJson()` method if you want to use NewtonsoftJson serialization. It is recommended to use the same serializer that you use in your ASP.NET application to avoid incorrect serialization/deserialization. By default `IFlueFlameHost` uses `System.Text.Json`.

## Step. 3: Your first test

Create a test class that inherits from `TestBase`.

Let's say your application already has an endpoint that returns all employees. The next test sends a `GET` request to `/api/employee/all` and checks that the response returned is `200 OK`:

```csharp
public class SimpleTests : TestBase
{
    [Test]
    public void GetReturnsOk()
    {
        Application
            .Http.Get
            .Url("/api/employee/all")
            .Send()
            .Response
                .AssertStatusCode(HttpStatusCode.OK);
    }
}
```

Learn more about REST API testing [here](/rest/basic.md)

## What's next?

You have familiarized yourself with the basic functionality of FlueFlame. Now you can [deep](/rest/basic.md) into testing your REST API and learn how to send and test received objects.

FlueFlame supports testing not only REST APIs, but also technologies such as [SignalR](/signalr/basic.md) and [gRPC](/grpc/basic.md). Over time, we will add the ability to test GraphQL and other popular technologies.