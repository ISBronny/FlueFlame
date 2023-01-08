# ASP.NET Core integration

You will most likely want to test your ASP.NET Core application.
The [FlueFlame.AspNetCore](https://www.nuget.org/packages/FlueFlame.AspNetCore/) package will help you with this.

FlueFlame will use `TestServer` from the `Microsoft.AspNetCore.Mvc.Testing` package.

FlueFlame create `TestServer` from `WebApplicationFactory`:

```csharp
var webApp = new WebApplicationFactory<Program>()
	.WithWebHostBuilder(builder =>
	{
		builder.ConfigureServices(services =>
		{
			//Configure your services here
		});
	});

```

::: tip

Since C# 9.0, the `Program` class does not exists if you use [Top-level statements](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9#top-level-statements). It can be declared by adding the following code in the end of `Program.cs` file:
```csharp
namespace Testing.TestData.AspNetCore
{
    public partial class Program {}
}
```

Also you can do that by either using the [InternalsVisibleToAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.internalsvisibletoattribute?view=net-6.0) in your main application project, or use this within the project file of your application where "ProjectName.Tests.EndToEnd" would be your testing project name:
```xml
  <ItemGroup>
    <InternalsVisibleTo Include="ProjectName.Tests.EndToEnd" />
  </ItemGroup>
```

And then use `global::` prefix:

```csharp
new WebApplicationFactory<global::Program>()
```
:::


You will most likely need to set up your application for testing purposes. For example, replace your database with InMemory:

```csharp
var webApp = new WebApplicationFactory<Program>()
	.WithWebHostBuilder(builder =>
	{
		builder.ConfigureServices(services =>
		{
			//Configure your services here
			var dbContextDescriptor = services.SingleOrDefault(
				d => d.ServiceType ==
				     typeof(DbContextOptions<EmployeeContext>));
			
			services.Remove(dbContextDescriptor);
			//Unique Database name for each test.
			var dbName = $"Employee_{Guid.NewGuid()}";
			
			//Use InMemory Database
			services.AddDbContext<EmployeeContext>(x => x.UseInMemoryDatabase(dbName));
		});
	});
```

Create `IFlueFlameHttpHost`:

```csharp
var httpHost = FlueFlameAspNetBuilder
	.CreateDefaultBuilder(webApp)
	.BuildHttpHost();
```

## TestBase class Example

The following is an example of the `TestBase` class for testing ASP.NET Core:

```csharp
public abstract class TestBase : IDisposable
{
	protected IFlueFlameHttpHost HttpHost { get; }
	protected IServiceProvider ServiceProvider { get; }
	protected TestServer TestServer { get; }

	protected TestBase()
	{
		var webApp = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder =>
			{
				builder.ConfigureServices(services =>
				{
					//Configure your services here
					var dbContextDescriptor = services.SingleOrDefault(
						d => d.ServiceType ==
						     typeof(DbContextOptions<EmployeeContext>));
					
					services.Remove(dbContextDescriptor);

					//Unique Database name for each test.
					var dbName = $"Employee_{Guid.NewGuid()}";
					
					//Use InMemory Database
					services.AddDbContext<EmployeeContext>(x => x.UseInMemoryDatabase(dbName));
				});
			});

		TestServer = webApp.Server;
		ServiceProvider = webApp.Services;

		var builder = FlueFlameAspNetBuilder.CreateDefaultBuilder(webApp)
			.ConfigureHttpClient(c =>
			{
				//Configure HttpClient for all FlueFlame hosts such as HttpHost, GrpcHost, SignalRHost...
				
				//Add JWT token to default request headers
				c.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetJwtToken()}");
			});

		HttpHost = builder.BuildHttpHost(b =>
		{
			//Use System.Text.Json serializer
			b.UseTextJsonSerializer();
			
			//Configure HttpClient only for FlueFlameHttpHost
			b.ConfigureHttpClient(client =>
			{
				
			});
		});
		
	}

	protected string GetJwtToken(string role = "admin", TimeSpan? lifetime = null)
	{
		var jwt = new JwtSecurityToken(
			issuer: AuthOptions.ISSUER,
			audience: AuthOptions.AUDIENCE,
			notBefore: DateTime.UtcNow,
			claims: new List<Claim>() { new(ClaimsIdentity.DefaultRoleClaimType, role) },
			expires: DateTime.UtcNow.Add(lifetime ?? TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
			signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
				SecurityAlgorithms.HmacSha256));

		return new JwtSecurityTokenHandler().WriteToken(jwt);
	}

	public void Dispose()
	{
		using var scope = ServiceProvider.CreateScope();
		var ctx = scope.ServiceProvider.GetRequiredService<EmployeeContext>();
		ctx.Database.EnsureDeleted();
		GC.SuppressFinalize(this);
	}
}
	
```
