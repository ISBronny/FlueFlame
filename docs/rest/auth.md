# Authorization

Most likely, your application uses authentication and you do not want to disable it for integration tests. FlueFlame provides several methods to solve this problem.

## Set Bearer token

You can set the authentication header before sending the request. The `WithBearerToken` method will set the `Authorization` header with your token.

```csharp
[Test]
public void AuthWithJwtReturnsOk()
{
    Application
        .Http.Get
        .Url("/api/admin/test")
        .WithBearerToken("token...")
        .Send()
        .Response
            .AssertStatusCode(HttpStatusCode.OK);
}
```

## Set Bearer token globaly

Setting an auth token on a per-request basis can be cumbersome, so you can add a default header for IFlueFlameHost in your TestBase class using the `AddDefaultBearerToken` method:

```csharp
public class TestBase
{
    protected readonly IFlueFlameHost Application;

    public TestBase()
    {
        //...

        Application = TestApplicationBuilder.CreateDefaultBuilder(webApp)
            .UseNewtonsoftJson()
            .Build();

        Application.Http.AddDefaultBearerToken("token...");
    }
}
```

