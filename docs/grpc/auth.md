# Authorization

You can add standard request headers for HttpClient in `IFlueFlameGrpcHost` config:

```csharp
var webApp = new WebApplicationFactory<Program>();
var builder = FlueFlameAspNetBuilder.CreateDefaultBuilder(webApp);

GrpcHost = builder.BuildGrpcHost(b =>
{
	//Configure HttpClient only for FlueFlameGrpcHost
	b.ConfigureHttpClient(client =>
	{
		client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetJwtToken()}");
	});
});
```

You can also use `Credentials` in `GrpcChannelOptions`:

```csharp
GrpcHost = builder.BuildGrpcHost(b =>
{

	//Use custom GrpcChannelOptions
	b.UseCustomGrpcChannelOptions(new GrpcChannelOptions()
	{
		MaxRetryAttempts = 1,
		Credentials = ChannelCredentials.Create(new SslCredentials(),
			CallCredentials.FromInterceptor((context, metadata) =>
			{
				metadata.Add("Authorization", $"Bearer {GetJwtToken()}");
				return Task.CompletedTask;
			}))
	});
});
```

Read more about authorization in the official [documentation](https://learn.microsoft.com/en-us/aspnet/core/grpc/authn-and-authz?view=aspnetcore-7.0)
