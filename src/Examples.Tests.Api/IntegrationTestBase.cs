using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Examples.Infrastructure.Auth;
using Examples.Infrastructure.Database;
using Examples.Api;
using FlueFlame.AspNetCore;
using FlueFlame.AspNetCore.Grpc;
using FlueFlame.Http.Host;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Examples.Tests.Api;

public abstract class IntegrationTestBase : IDisposable
{
	protected IFlueFlameHttpHost HttpHost { get; }
	protected IFlueFlameGrpcHost GrpcHost { get; }
	protected IServiceProvider ServiceProvider { get; }
	protected TestServer TestServer { get; }
	protected EmployeeContext EmployeeContext => ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<EmployeeContext>();

	protected IntegrationTestBase()
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

					var dbName = $"Employee_{Guid.NewGuid()}";
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
		
		GrpcHost = builder.BuildGrpcHost(b =>
		{
			//Configure FlueFlameGrpcHost here
			
			//Configure HttpClient only for FlueFlameGrpcHost
			b.ConfigureHttpClient(client =>
			{

			});

			//Use custom GrpcChannelOptions
			b.UseCustomGrpcChannelOptions(new GrpcChannelOptions()
			{
				MaxRetryAttempts = 1
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
	