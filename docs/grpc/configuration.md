# Configure IFlueFlameGrpcHost

The configuration of `IFlueFlameGrpcHost` is similar to `IFlueFlameHttpHost`. [More](/rest/configuration.md)

## HttpClient Configuration

With the `ConfigureHttpClient` method, you have full access to `HttpClient` and can configure it however you like.

```csharp

var webApp = new WebApplicationFactory<Program>();
var builder = FlueFlameAspNetBuilder.CreateDefaultBuilder(webApp)

GrpcHost = builder.BuildGrpcHost(b =>
{
	b.ConfigureHttpClient(client =>
	{
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetJwtToken()}");
	});
});
```

## Custom HttpClient

If you have your own `HttpClient` you can force `FlueFlame` to use it:

```csharp
GrpcHost = builder.BuildGrpcHost(b =>
{
	b.UseCustomHttpClient(new HttpClient()
	{
		Timeout = TimeSpan.FromMilliseconds(100)
	});
});
```

## Custom GrpcChannelOptions

```csharp
GrpcHost = builder.BuildGrpcHost(b =>
{
	b.UseCustomGrpcChannelOptions(new GrpcChannelOptions()
	{
		MaxRetryAttempts = 1,
        LoggerFactory = new NullLoggerFactory()
	});
});
```


