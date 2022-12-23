using Examples.Grpc;
using Examples.Infrastructure.Auth;
using Examples.Infrastructure.Database;
using Examples.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using EmployeeService = Examples.Api.Services.EmployeeService;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<EmployeeContext>(x => x.UseInMemoryDatabase("Employee"));
services.AddControllers();

services.AddScoped<IEmployeeRepository, EmployeeRepository>();

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.RequireHttpsMetadata = false;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = AuthOptions.ISSUER,
			ValidateAudience = true,
			ValidAudience = AuthOptions.AUDIENCE,
			ValidateLifetime = true,
			IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
			ValidateIssuerSigningKey = true,
		};
	});

//services.Configure<MvcOptions>(x => x.OutputFormatters.Add(new XmlSerializerOutputFormatter()));

services.AddGrpc();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

#pragma warning disable ASP0014
app.UseEndpoints(b =>
{
	b.MapControllers();
	b.MapGrpcService<EmployeeService>();
});
#pragma warning restore ASP0014




app.Run();
namespace Examples.Api
{
	public partial class Program
	{
	
	}
}