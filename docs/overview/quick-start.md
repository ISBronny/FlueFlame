# Quick start

## TestBase

Create a NUnit or xUnit project. FlueFlame supports both frameworks. In this example, NUnit will be used.

Create base test class:

```csharp
public abstract class TestBase
{
    protected readonly FlueFlameHost Application;

    public TestBase()
    {
        var webApp = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {

            });
        
        Application = TestApplicationBuilder.CreateDefaultBuilder(webApp)
            .UseNewtonsoftJson()
            .Build();
    }
}
```
## WebApplicationFactory

Create the WebApplicationFactory needed to create the `FlueFlameHost`.

```csharp
var webApp = new WebApplicationFactory<Program>()
    .WithWebHostBuilder(builder =>
    {

    });
```

In the `WithWebHostBuilder` method, you can add or remove the used services. Read more in [Builder](/overview/builder.md).

> Since .NET 6, the `Program` class has been deprecated. It can be declared by adding the following code in the end of `Program.cs` file:
>```csharp
>namespace Testing.TestData.AspNetCore
>{
>    public partial class Program {}
>}
>```

## TestApplicationBuilder

Create `FlueFlameHost` using `TestApplicationBuilder`. If you want your models to be serialized and deserialized using Newtonsoft.Json, call the `UseNewtonsoftJson()` method. The default will be `System.Text.Json`.

```csharp
Application = TestApplicationBuilder.CreateDefaultBuilder(webApp)
            .UseNewtonsoftJson()
            .Build();
```

## Simple Test

Сreate a simple test that sends a `GET` request to `/api/employee/all` and checks that the response returned is `200 OK`

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
