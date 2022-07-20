using Microsoft.EntityFrameworkCore;
using Testing.TestData.AspNetCore.Database;
using Testing.TestData.AspNetCore.Repositories;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddDbContext<EmployeeContext>(x => x.UseInMemoryDatabase("Employee"));
services.AddControllers()
    .AddJsonOptions(x=> {});

services.AddScoped<IEmployeeRepository, EmployeeRepository>();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

namespace Testing.TestData.AspNetCore
{
    public partial class Program {}
}