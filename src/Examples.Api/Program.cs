using Examples.Infrastructure.Auth;
using Examples.Infrastructure.Database;
using Examples.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<EmployeeContext>(x => x.UseInMemoryDatabase("Employee"));
services.AddControllers()
	.AddNewtonsoftJson();

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

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
	//endpoints.MapGrpcService<GreatMathService>();
});

app.Run();
namespace Examples.Api
{
	public partial class Program
	{
	
	}
}