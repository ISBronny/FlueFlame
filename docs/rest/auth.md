# Authorization

Most likely, your application uses authentication and you do not want to disable it for integration tests. FlueFlame provides several methods to solve this problem.

## Set Bearer token once

You can set the authentication header before sending the request. The `WithJwtToken` method will set the `Authorization` header with your token.

```csharp
[Test]
public void AuthWithJwtReturnsOk()
{
    HttpHost.Get
        .Url("/api/admin/test")
        .WithJwtToken(YourToken)
        .Send()
        .Response
            .AssertStatusCode(HttpStatusCode.OK);
}
```

## Set Bearer token globaly

Setting an auth token on a per-request basis can be cumbersome, so you can add a default header for IFlueFlameHttpHost in your TestBase class:

```csharp
public class TestBase
{
    protected IFlueFlameHttpHost HttpHost { get; }

    public TestBase()
    {
        //...

        HttpHost = builder.BuildHttpHost(b =>
		{
			//Configure HttpClient only for FlueFlameHttpHost
			b.ConfigureHttpClient(client =>
			{
				client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetJwtToken()}");
			});
		});
    }
}
```

