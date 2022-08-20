using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Testing.TestData.AspNetCore.Auth;
using Testing.TestData.AspNetCore.Database;
using Testing.TestData.AspNetCore.Hubs;
using Testing.TestData.AspNetCore.Repositories;
using Testing.TestData.AspNetCore.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddDbContext<EmployeeContext>(x => x.UseInMemoryDatabase("Employee"));
services.AddControllers()
    .AddJsonOptions(x=> {});

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

services.Configure<MvcOptions>(x => x.OutputFormatters.Add(new XmlSerializerOutputFormatter()));
services.AddSignalR();
services.AddGrpc();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapGrpcService<GreatMathService>();
    endpoints.MapGrpcService<GreeterService>();
    
    endpoints.MapHub<PingHub>("/hub/ping");
    endpoints.MapHub<ChatHub>("/hub/chat");
});

app.Run();

namespace Testing.TestData.AspNetCore
{
    public partial class Program {}
}