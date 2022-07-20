using FlueFlame.AspNetCore;
using FlueFlame.AspNetCore.Factories;
using Microsoft.AspNetCore.Mvc.Testing;
using Testing.TestData.AspNetCore;
using Testing.TestData.AspNetCore.Models;

namespace Testing.Tests.AspNet.xUnit;

public class UnitTest1
{
    protected readonly FlueFlameHost Application;

    public UnitTest1()
    {
        var webApp = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {

            });
        
        Application = TestApplicationBuilder.CreateDefaultBuilder(webApp)
            .UseNewtonsoftJson()
            .Build();
    }
    
    [Fact]
    public void Test1()
    {
        var employee = new Employee()
        {
            Age = 12,
            Position = "Middle Python Developer",
            FullName = "Max Power"
        };
         
    }
}