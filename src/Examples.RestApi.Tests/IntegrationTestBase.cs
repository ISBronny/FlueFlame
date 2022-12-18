using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks.Dataflow;
using Examples.RestApi.Auth;
using FlueFlame.AspNetCore;
using FlueFlame.Http.Host;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;

namespace Examples.RestApi.Tests;

public abstract class IntegrationTestBase
{
	protected IFlueFlameHttpHost HttpHost { get; }
	protected IServiceProvider ServiceProvider { get; }

	protected IntegrationTestBase()
	{
		var webApp = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder =>
			{
				builder.ConfigureServices(services =>
				{
					//Configure your services here
				});
			});

		ServiceProvider = webApp.Services;

		var builder = FlueFlameAspNetBuilder.CreateDefaultBuilder(webApp);

		HttpHost = builder.BuildHttpHost(b =>
		{
			//Use System.Text.Json serializer
			b.UseTextJsonSerializer();
		});

		//Add JWT token to default request headers
		HttpHost.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetJwtToken()}");
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
}
	